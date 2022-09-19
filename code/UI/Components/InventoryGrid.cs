using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Tests;
using ZenWorks.Items;
using ZenWorks.Items.Foods;

namespace ZenWorks.UI.Components
{
	public partial class InventoryGrid : Panel
	{
		private VirtualScrollPanel Canvas;
		public Vector2 Size { get; private set; }
		
		public Vector2 SizeItem { get; private set; }
		
		public bool IsDragging { get; set; } = false;
		public bool IsFinishDragging { get; set; } = false;

		public int DragIndex { get; set; } = -1;
		public int DragTargetIndex { get; set; } = -1;

		public InventoryGrid( string title, Vector2 size, Vector2 sizeItem, Dictionary<int, Item> items = null )
		{
			Size = size;
			SizeItem = sizeItem;
			
			StyleSheet.Load( "./UI/Components/InventoryGrid.scss" );
			AddClass( "InventoryGrid" );
			AddChild( out Canvas, "canvas" );

			Canvas.Layout.ItemWidth = (int)sizeItem.x;
			Canvas.Layout.ItemHeight = (int)sizeItem.y;
			Canvas.Layout.Columns = (int)size.x;

			Canvas.OnCreateCell = ( cell, data ) =>
			{
				var square = (InventorySquare)data;
				cell.AddChild( square );
			};

			if ( items == null )
			{
				for ( int i = 0; i < size.x * size.y; i++ )
					Canvas.Data.Add( new InventorySquare( this, null, i, true ) );
			} else Build( items, true );
		}

		public void Build( Dictionary<int, Item> items, bool withAnimation = true )
		{
			Canvas.Data.Clear();

			foreach ( var item in items )
				Canvas.Data.Add( new InventorySquare( this, item.Value, item.Key, withAnimation ) );

			Canvas.NeedsRebuild = true;
		}
		
		public int Count()
		{
			return Canvas.Data.FindAll( x => (x as InventorySquare)?.Item != null ).Count();
		}

		public async override void Delete( bool immediate = false )
		{
			if ( !immediate )
			{
				foreach ( InventorySquare square in Canvas.Data )
					square.AddClass( "EnableAnimation" );

				foreach ( InventorySquare square in Canvas.Data )
					square.Delete();

				await Task.DelaySeconds( 1f );
			}
			
			base.Delete( immediate );
		}

		public override void Tick()
		{
			base.Tick();
			
			if ( !IsDragging && IsFinishDragging && !Input.Down( InputButton.Run ) )
			{
				if ( DragIndex != -1 && DragTargetIndex != -1 )
				{
					ConsoleSystem.Run( "zw_move_item_inventory", DragIndex, DragTargetIndex );
					DragIndex = -1;
					DragTargetIndex = -1;
					IsDragging = false;
					IsFinishDragging = false;
				}
				else
				{
					DragIndex = -1;
					DragTargetIndex = -1;
					IsDragging = false;
					IsFinishDragging = false;
					Event.Run( "zw_main_inventory.refresh" );
				}
			}
		}
		
	}

	public partial class InventorySquare : Panel
	{

		private TimeSince TimeSinceCreated;
		public Item Item { get; private set; }
		public int Index { get; private set; }

		private Angles CamAngles;
		public SceneWorld SceneWorld { get; private set; }
		public SceneObject itemPreview { get; private set; }
		
		public Label ItemStack { get; private set; }
		
		public ScenePanel ScenePanel { get; private set; }
		public InventoryGrid Inventory { get; private set; }
		public Label Stack { get; private set; }

		public InventorySquare( InventoryGrid inventory, Item item, int index, bool withAnimation = true)
		{
			AddClass( "InventorySquare" );
			StyleSheet.Load( "./UI/Components/InventoryGrid.scss" );

			Inventory = inventory;
			
			Item = item;
			Index = index;

			TimeSinceCreated = 0f;

			if ( item != null )
			{
				AddClass( "HasItem" );

				SceneWorld = new SceneWorld();
				itemPreview = new SceneObject( SceneWorld, Model.Load( item.Model ), Transform.Zero );

				var light = new SceneLight( SceneWorld, new Vector3( -100, 100, 150 ), 2000, Color.White );
				light.Falloff = 0;
				//light.Falloff = 0f;
				light = new SceneLight( SceneWorld, new Vector3( 700, -30, 170 ), 2000, Color.White );
				light.Falloff = 0;

				//light.Falloff = 0.2f; 
				light = new SceneLight( SceneWorld, new Vector3( 100, 100, 150 ), 2000, Color.White );
				light.Falloff = 0;

				ScenePanel = Add.ScenePanel( SceneWorld, new Vector3( 175, 0, 30 ), Rotation.From( CamAngles ), 10 );
				ScenePanel.Style.Height = (int)Inventory.SizeItem.y;
				ScenePanel.Style.Width = (int)Inventory.SizeItem.x;

				Angles angles = new(25, 180, 0);
				Vector3 pos = Vector3.Up * 10 + angles.Direction * -200;

				ScenePanel.World = SceneWorld;
				ScenePanel.CameraPosition = pos;
				ScenePanel.CameraRotation = Rotation.From( angles );
				ScenePanel.FieldOfView = 10;
				ScenePanel.AmbientColor = Color.Gray * 0.2f;
				
				if ( item.IsStackable() || item.Stack > 1 )
					ItemStack = ScenePanel.Add.Label( Item.Stack.ToString(), "StackableText" );
			}
			
			if ( withAnimation ) AddClass( "EnableAnimation" );
		}

		public override void Tick()
		{
			base.Tick();

			if ( Item != null )
			{
				if ( Item.IsStackable() || Item.Stack > 1 )
					ItemStack.Text = Item.Stack.ToString();
			}

			if ( TimeSinceCreated <= -1 )
			{
				TimeSinceCreated = -1;
				return;
			}

			if ( TimeSinceCreated < 1f ) return;
			
			RemoveClass( "EnableAnimation" );
			TimeSinceCreated = -1;
		}

		protected override void OnDragSelect( SelectionEvent e )
		{
			base.OnDragSelect( e );
			
			if ( Item == null ) return;

			ScenePanel.Style.Position = PositionMode.Absolute;
			ScenePanel.Style.Top = Length.Pixels(MousePosition.y / ScaleToScreen - (Inventory.SizeItem.y / 2));
			ScenePanel.Style.Left = Length.Pixels(MousePosition.x / ScaleToScreen - (Inventory.SizeItem.x / 2));
			ScenePanel.Style.ZIndex = 999999999;
			ScenePanel.Style.PointerEvents = PointerEvents.None;
		}

		protected override void OnDoubleClick( MousePanelEvent e )
		{
			base.OnDoubleClick( e );
			
			if ( 
				Item == null || 
				e.Button != "mouseleft" || 
				!Item.IsStackable() ||
				Item.Stack <= 1 ||
				Inventory.Count() >= Inventory.Size.x * Inventory.Size.y
			) return;
			
			ConsoleSystem.Run( "zw_split_item_inventory", Index );
		}

		protected override void OnRightClick( MousePanelEvent e )
		{
			base.OnRightClick( e );

			var hud = Local.Hud;
			
			if ( Item == null || hud == null ) return;
			
			if (TabMenu.Menu.Current.MenuInventory.ChildrenOfType<InventorySquareTooltip>()?.Count() >= 1)
				TabMenu.Menu.Current.MenuInventory.ChildrenOfType<InventorySquareTooltip>().ToArray()[0].Delete( true );

			TabMenu.Menu.Current.MenuInventory.AddChild( new InventorySquareTooltip( this, Item ) );
		}

		protected override void OnMouseDown( MousePanelEvent e )
		{
			base.OnMouseDown( e );
			if ( Item == null || e.Button != "mouseleft" || Input.Down( InputButton.Run ) ) return;
			Inventory.IsDragging = true;
			Inventory.DragIndex = Index;
			Style.BackgroundColor = "transparent";
			ScenePanel.Style.BackgroundColor = new Color( 0.0f, 0.5f, 0.0f, 0.5f );
		}

		protected override void OnMouseUp( MousePanelEvent e )
		{
			base.OnMouseUp( e );
			if ( Item == null ) return;
			Inventory.IsDragging = false;
			Inventory.IsFinishDragging = true;
		}

		protected override void OnMouseOver( MousePanelEvent e )
		{
			base.OnMouseOver( e );
			if ( Inventory.IsDragging && Inventory.DragIndex != -1 && !Inventory.IsFinishDragging )
				Inventory.DragTargetIndex = Index;
		}
	}

	public class InventorySquareTooltip : Panel
	{
		public InventorySquareTooltip( InventorySquare square, Item item )
		{
			if ( item == null ) return;
			
			Style.Top = Length.Pixels( MousePosition.y / ScaleToScreen - (square.Inventory.SizeItem.y / 2) );
			Style.Left = Length.Pixels( MousePosition.x / ScaleToScreen - (square.Inventory.SizeItem.x / 2) );

			StyleSheet.Load( "./UI/Components/InventoryGrid.scss" );

			if ( item is Food food )
			{
				if ( food.CanDrink )
					Add.Button( "Boire", "Button", () =>
					{
						ConsoleSystem.Run( "zw_use_item_inventory", square.Index, "drink", false );
						Delete( true );
					} );

				if ( food.CanEat )
					Add.Button( "Manger", "Button", () =>
					{
						ConsoleSystem.Run( "zw_use_item_inventory", square.Index, "eat", false );
						Delete( true );
					} );
			}

			if ( item.CanDrop )
			{
				Add.Button("Jeter", "Button", () =>
				{
					ConsoleSystem.Run( "zw_use_item_inventory", square.Index, "drop", false);
					Delete( true );
				} );

				if ( item.IsStackable() && item.Stack > 1 ) 
					Add.Button($"Jeter les {item.Stack}", "Button", () =>
					{
						ConsoleSystem.Run( "zw_use_item_inventory", square.Index, "drop", true);
						Delete( true );
					} );
			}
				
		}
	}
}
