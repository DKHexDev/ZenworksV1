using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Sandbox;
using Sandbox.UI;
using ZenWorks.Data.Modals;
using ZenWorks.Entities;
using ZenWorks.Factions;
using ZenWorks.Items;
using ZenWorks.UI.TabMenu;
using Menu = ZenWorks.UI.TabMenu.Menu;

namespace ZenWorks
{
	public partial class Character : Player
	{
		private TimeSince FootStepTimeSince;
		private TimeSince FootStepRunTimeSince;
		private TimeSince StaminaTimeSince;

		private TimeSince HungerTimeSince;
		private TimeSince ThirstTimeSince;

		/// <summary>
		/// Name of the character
		/// </summary>
		[Net]
		public string Fullname { get; set; } = null;

		/// <summary>
		/// Description of the character
		/// </summary>
		[Net]
		public string Description { get; set; } = null;

		/// <summary>
		/// Money of the character
		/// </summary>
		[Net, Local]
		public float Money { get; set; } = 0f;

		/// <summary>
		/// Armor of the character
		/// </summary>
		[Net]
		public float Armor { get; set; } = 0f;

		/// <summary>
		/// Stamina of the character
		/// </summary>
		[Net]
		public float Stamina { get; set; } = 100f;

		/// <summary>
		/// Hunger of the character
		/// </summary>
		[Net]
		public float Hunger { get; set; } = 100f;

		/// <summary>
		/// Thirst of the character
		/// </summary>
		[Net]
		public float Thirst { get; set; } = 100f;

		public Character()
		{
			Inventory = new Inventory();
		}
		
		public Character( CharacterData data = null )
		{
			Tags.Add( "character" );

			if ( data != null )
			{
				Fullname = data.Name;
				Description = data.Description;
				Money = data.Money;
				Hunger = data.Hunger;
				Thirst = data.Thirst;
				Armor = data.Armor;
				SetFaction( Factions.Faction.GetFaction( data.Faction ) );
				IndexData = data.IndexData;
			}
		}

		/// <summary>
		/// Method when spawn the character
		/// </summary>
		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/citizen/citizen.vmdl" );

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			MoveType = MoveType.Physics;
			CollisionGroup = CollisionGroup.Player;

			Controller = new WalkController();
			Animator = new StandardPlayerAnimator();
			CameraMode = new ThirdPersonCamera();
			Inventory = new Inventory();

			if ( Controller is WalkController controller )
			{
				controller.WalkSpeed = 90.0f;
				controller.DefaultSpeed = 150.0f;
				controller.SprintSpeed = 250.0f;
			}
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if ( LifeState != LifeState.Alive )
				return;

			TickPlayerUse();

			if ( Input.Pressed( InputButton.View ) )
			{
				if ( CameraMode is not FirstPersonCamera )
				{
					CameraMode = new FirstPersonCamera();
				}
				else
				{
					CameraMode = new ThirdPersonCamera();
				}
			}

			if (
				FootStepTimeSince > 0.5f &&
				!Input.Down( InputButton.Run ) &&
				!Input.Down( InputButton.Walk ) &&
				(Input.Down( InputButton.Right ) ||
				 Input.Down( InputButton.Left ) ||
				 Input.Down( InputButton.Forward ))
			)
			{
				using ( Prediction.Off() )
				{
					var sound = Sound.FromEntity( Faction?.FootstepSound, this );
					sound.SetVolume( 0.5f );
				}

				FootStepTimeSince = 0;
			}

			if (
				Stamina > 0 &&
				Input.Down( InputButton.Run ) &&
				(Input.Down( InputButton.Forward ) ||
				 Input.Down( InputButton.Left ) ||
				 Input.Down( InputButton.Right ))
			)
			{
				if ( FootStepRunTimeSince > 0.4f )
				{
					using ( Prediction.Off() )
					{
						var sound = Sound.FromEntity( Faction?.FootstepRunSound, this );
						sound.SetVolume( 1f );
					}

					FootStepRunTimeSince = 0;
				}

				if ( StaminaTimeSince > 0.2f && Stamina > 0 )
				{
					Stamina -= 5;

					if ( Stamina < 0 )
						Stamina = 0;

					StaminaTimeSince = 0;
				}
			}
			else
			{
				if ( !Input.Down( InputButton.Run ) && StaminaTimeSince > 0.2f && Stamina < 100 )
				{
					Stamina += 2;

					if ( Stamina > 100 )
						Stamina = 100;

					StaminaTimeSince = 0;
				}

				if ( Stamina <= 0 && FootStepRunTimeSince > 0.5f )
				{
					using ( Prediction.Off() )
					{
						var sound = Sound.FromEntity( Faction?.FootstepRunSound, this );
						sound.SetVolume( 0.5f );
					}

					FootStepRunTimeSince = 0;
				}
			}

			if ( Controller is WalkController controller )
			{
				if ( Stamina <= 0 )
				{
					controller.SprintSpeed = 150.0f;
				}
				else if ( Stamina <= 100 && Stamina >= 1 )
				{
					controller.SprintSpeed = 250.0f;
				}
			}

			// Always 1 hour
			if ( HungerTimeSince > 3600f )
			{
				Hunger -= Rand.Float( 5f, 10f );
				HungerTimeSince = 0;
			}

			// Always 40 minutes
			if ( ThirstTimeSince > 2400f )
			{
				Thirst -= Rand.Float( 5f, 10f );
				ThirstTimeSince = 0;
			}
			
			if ( IsClient && Input.Pressed( InputButton.Score ) )
			{
				if ( Local.Hud.ChildrenOfType<Menu>()?.Count() > 0 ) Local.Hud.ChildrenOfType<Menu>().First().Delete();
				else Local.Hud.AddChild<Menu>();
			}
		}
	}
}


