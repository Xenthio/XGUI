using Sandbox;
using Sandbox.UI;
using System.Linq;
namespace XGUI;

public partial class Window : Panel
{
	public Panel TitleBar { get; set; } = new Panel();
	public Label TitleLabel { get; set; } = new Label();
	public Panel TitleIcon { get; set; } = new Panel();
	public Panel TitleSpacer { get; set; } = new Panel();

	public Vector2 Position;
	public Vector2 Size;
	public Vector2 MinSize = new Vector2();

	public int ZIndex;

	public bool HasControls = true;

	public bool HasMinimise = false;
	public bool HasMaximise = false;
	public bool HasClose = true;

	public bool IsResizable;

	public Button ControlsClose { get; set; } = new Button();
	public Button ControlsMinimise { get; set; } = new Button();
	public Button ControlsMaximise { get; set; } = new Button();


	public Window()
	{

		AddClass( "Panel" );
		AddClass( "Window" );
		Style.PointerEvents = PointerEvents.All;
		Style.Position = PositionMode.Absolute;
		Style.FlexDirection = FlexDirection.Column;
	}
	protected override void OnAfterTreeRender( bool firstTime )
	{
		base.OnAfterTreeRender( firstTime );
		if ( firstTime )
		{
			CreateTitleBar();
			this.AddEventListener( "onmousedown", ResizeDown );
			this.AddEventListener( "onmouseup", ResizeUp );
			this.AddEventListener( "onmousemove", ResizeMove );
			OverrideButtons();
		}
		SetChildIndex( TitleBar, 0 );
	}

	public void OverrideButtons()
	{
		foreach ( Panel button in Descendants.OfType<Button>() )
		{
			var focusallowed = button.GetAttribute( "focus", "0" );
			if ( focusallowed == "1" )
			{
				button.AcceptsFocus = true;
			}
			var autofocus = button.GetAttribute( "autofocus", "0" );
			if ( autofocus == "1" )
			{
				button.Focus();
			}
		}
	}
	Panel LastFocus;
	public void FocusUpdate()
	{
		if ( InputFocus.Current == null || InputFocus.Current is Window ) return;
		if ( InputFocus.Current != LastFocus )
		{
			if ( LastFocus != null )
			{
				LastFocus.SetClass( "focus", false );
			}
			LastFocus = InputFocus.Current;
			LastFocus.SetClass( "focus", true );
		}
	}
	public void CreateTitleBar()
	{
		/*
		<style>
			Window {
				pointer-events:all;
				position: absolute;
				flex-direction:column;
				.TitleBar {
					.TitleIcon {

					}
					.TitleSpacer {
						flex-grow: 1;
						background-color: rgba(0,0,0,1);
					}
					.Control {
					}
				}
			}
		</style>
		<div class="TitleBar" @ref=TitleBar>
			<div class="TitleIcon" @ref=TitleIcon></div>
			<div>@Title</div>
			<div class="TitleSpacer" onmousedown=@DragBarDown onmouseup=@DragBarUp onmousemove=@Drag></div>
			<button class="Control" @ref=ControlsClose onclick=@Close>X</button>
		</div>
		*/


		TitleBar.AddClass( "TitleBar" );
		TitleIcon.AddClass( "TitleIcon" );
		TitleLabel.AddClass( "TitleLabel" );
		TitleSpacer.AddClass( "TitleSpacer" );
		ControlsClose.AddClass( "Control" );
		ControlsClose.AddClass( "CloseButton" );
		ControlsMinimise.AddClass( "Control" );
		ControlsMinimise.AddClass( "MinimiseButton" );
		ControlsMaximise.AddClass( "Control" );
		ControlsMaximise.AddClass( "MaximiseButton" );

		AddChild( TitleBar );
		TitleBar.AddChild( TitleIcon );
		TitleBar.AddChild( TitleLabel );
		TitleBar.AddChild( TitleSpacer );
		TitleBar.Style.ZIndex = 100;
		if ( HasMinimise ) TitleBar.AddChild( ControlsMinimise );
		if ( HasMaximise ) TitleBar.AddChild( ControlsMaximise );
		if ( HasClose ) TitleBar.AddChild( ControlsClose );


		TitleSpacer.AddEventListener( "onmousedown", DragBarDown );
		TitleSpacer.AddEventListener( "onmouseup", DragBarUp );
		TitleSpacer.AddEventListener( "onmousedrag", Drag );
		TitleSpacer.Style.FlexGrow = 1;

		ControlsMinimise.AddEventListener( "onclick", Minimise );
		ControlsMinimise.Text = "0";

		ControlsMaximise.AddEventListener( "onclick", Maximise );
		ControlsMaximise.Text = "1";

		ControlsClose.AddEventListener( "onclick", Close );
		ControlsClose.Text = "r";

	}

	bool Minimised = false;
	Vector2 PreMinimisedSize;
	Vector2 PreMinimisedPos;
	public void Minimise()
	{
		if ( !Minimised )
		{
			PreMinimisedSize = Box.Rect.Size;

			PreMinimisedPos = Position;

			Position.x = 0;

			var newheight = TitleBar.Box.Rect.Size.y + ((TitleBar.Box.Rect.Position.y - Box.Rect.Position.y) * 2);
			Log.Info( newheight );
			Position.y = Parent.Box.Rect.Size.y - newheight;

			Style.Height = newheight;
			Style.Width = 128;
			Minimised = true;
		}
		else
		{
			Minimised = false;
			Style.Width = PreMinimisedSize.x;
			Style.Height = PreMinimisedSize.y;

			Position = PreMinimisedPos;
		}
		Log.Info( "minimise" );
	}

	bool Maximised = false;
	Vector2 PreMaximisedSize;
	Vector2 PreMaximisedPos;
	public void Maximise()
	{
		if ( !Maximised )
		{
			PreMaximisedSize = Box.Rect.Size;

			PreMaximisedPos = Position;

			Position = 0;

			Style.Height = Parent.Box.Rect.Size.y;
			Style.Width = Parent.Box.Rect.Size.x;
			Maximised = true;
		}
		else
		{
			Maximised = false;
			Style.Width = PreMaximisedSize.x;
			Style.Height = PreMaximisedSize.y;

			Position = PreMaximisedPos;
		}
		Log.Info( "maximise" );
	}
	public void Close()
	{
		Log.Info( "close" );
		Delete();
	}

	public override void Tick()
	{
		base.Tick();
		Drag();

		if ( Style.Left == null )
		{
			Style.Left = 0;
			Style.Top = 0;
		}
		Style.Position = PositionMode.Absolute;
		Style.Left = Position.x * ScaleFromScreen;
		Style.Top = Position.y * ScaleFromScreen;

		Style.ZIndex = (Parent.ChildrenCount - Parent.GetChildIndex( this )) * 10;

		SetClass( "unfocused", !this.HasFocus );
		FocusUpdate();
	}

	// -------------
	// Dragging
	// -------------
	bool Dragging = false;
	float xoff = 0;
	float yoff = 0;
	public void Drag()
	{
		if ( !Dragging ) return;
		Position.x = ((Parent.MousePosition.x) - xoff);
		Position.y = ((Parent.MousePosition.y) - yoff);

		// Window edge to edge snapping
		foreach ( Window window in Parent.Children.OfType<Window>() )
		{
			var snapDistance = 10;

			var window1leftpos = Position.x;
			var window1rightpos = Position.x + Box.Rect.Size.x;
			var window1uppos = Position.y;
			var window1downpos = Position.y + Box.Rect.Size.y;

			var window2leftpos = window.Position.x;
			var window2rightpos = window.Position.x + window.Box.Rect.Size.x;
			var window2uppos = window.Position.y;
			var window2downpos = window.Position.y + window.Box.Rect.Size.y;

			if ( !(window1downpos < window2uppos || window1uppos > window2downpos) )
			{
				if ( window1leftpos.AlmostEqual( window2rightpos, snapDistance ) ) Position.x -= window1leftpos - window2rightpos;
				if ( window1rightpos.AlmostEqual( window2leftpos, snapDistance ) ) Position.x += window2leftpos - window1rightpos;
			}
			if ( !(window1rightpos < window2leftpos || window1leftpos > window2rightpos) )
			{
				if ( window1uppos.AlmostEqual( window2downpos, snapDistance ) ) Position.y -= window1uppos - window2downpos;
				if ( window1downpos.AlmostEqual( window2uppos, snapDistance ) ) Position.y += window2uppos - window1downpos;
			}
		}
	}
	public void DragBarDown()
	{
		xoff = (float)((FindRootPanel().MousePosition.x) - Box.Rect.Left);
		yoff = (float)((FindRootPanel().MousePosition.y) - Box.Rect.Top);
		Dragging = true;
	}
	public void DragBarUp()
	{
		Dragging = false;
	}

	// -------------


	// -------------
	// Focusing
	// -------------

	protected override void OnMouseDown( MousePanelEvent e )
	{
		AcceptsFocus = true;
		if ( !HasFocus )
		{
			Focus();
		}
		Parent.SetChildIndex( this, 0 );
		//Parent.SortChildren( x => x.HasFocus ? 1 : 0 );
		base.OnMouseDown( e );
	}
	// -------------
	// Resizing
	// ------------- 
	internal bool draggingR = false;
	internal bool draggingL = false;
	internal bool draggingT = false;
	internal bool draggingB = false;

	public void ResizeDown()
	{
		var Distance = 5;
		var mousePos = FindRootPanel().MousePosition;
		if ( mousePos.y.AlmostEqual( this.Box.Rect.Bottom, Distance ) ) draggingB = true;
		if ( mousePos.x.AlmostEqual( this.Box.Rect.Right, Distance ) ) draggingR = true;
		if ( mousePos.y.AlmostEqual( this.Box.Rect.Top, Distance ) ) draggingT = true;
		if ( mousePos.x.AlmostEqual( this.Box.Rect.Left, Distance ) ) draggingL = true;
		//draggingT = true;
		//draggingL = true;
		xoff1 = (float)((FindRootPanel().MousePosition.x) - this.Box.Rect.Right);
		yoff1 = (float)((FindRootPanel().MousePosition.y) - this.Box.Rect.Bottom);
		xoff2 = (float)((FindRootPanel().MousePosition.x) - this.Box.Rect.Left);
		yoff2 = (float)((FindRootPanel().MousePosition.y) - this.Box.Rect.Top);
	}
	public void ResizeUp()
	{
		draggingB = false;
		draggingR = false;
		draggingT = false;
		draggingL = false;
	}
	internal float xoff1 = 0;
	internal float yoff1 = 0;
	internal float xoff2 = 0;
	internal float yoff2 = 0;
	public void ResizeMove()
	{
		var Distance = 5;
		var mousePos = FindRootPanel().MousePosition;

		if ( mousePos.y.AlmostEqual( this.Box.Rect.Bottom, Distance ) || draggingB ) Style.Cursor = "move";
		else if ( mousePos.x.AlmostEqual( this.Box.Rect.Right, Distance ) || draggingR ) Style.Cursor = "move";
		else if ( mousePos.y.AlmostEqual( this.Box.Rect.Top, Distance ) || draggingT ) Style.Cursor = "move";
		else if ( mousePos.x.AlmostEqual( this.Box.Rect.Left, Distance ) || draggingL ) Style.Cursor = "move";
		else Style.Cursor = "unset";

		/*if ( Mouse )
		{
			ResizeUp( e );
		}*/

		if ( draggingB )
		{
			//Parent.Style.Width = (FindRootPanel().MousePosition.x - Parent.Box.Rect.Left) - xoff;
			var newheight = (FindRootPanel().MousePosition.y - Box.Rect.Top) - yoff1;
			if ( newheight > MinSize.y )
			{
				Style.Height = newheight;
			}
		}

		if ( draggingR )
		{
			var newwidth = (FindRootPanel().MousePosition.x - Box.Rect.Left) - xoff1;
			if ( newwidth > MinSize.x )
			{
				Style.Width = newwidth;
			}
			//Parent.Style.Height = (FindRootPanel().MousePosition.y - Parent.Box.Rect.Top) - yoff;
		}
		if ( draggingT )
		{
			var newheight = Box.Rect.Height - ((FindRootPanel().MousePosition.y - yoff2) - Box.Rect.Top);
			if ( newheight > MinSize.y )
			{
				Style.Height = newheight;
				Position.y = FindRootPanel().MousePosition.y - yoff2;
			}
		}

		if ( draggingL )
		{
			var newwidth = Box.Rect.Width - ((FindRootPanel().MousePosition.x - xoff2) - Box.Rect.Left);
			if ( newwidth > MinSize.x )
			{
				Style.Width = newwidth;
				Position.x = FindRootPanel().MousePosition.x - xoff2;
			}
		}


	}
	// -------------
	public override void SetProperty( string name, string value )
	{
		switch ( name )
		{
			case "title":
				{
					TitleLabel.Text = value;
					return;
				}
			case "hasminimise":
				{
					HasMinimise = bool.Parse( value );
					return;
				}
			case "hasmaximise":
				{
					HasMaximise = bool.Parse( value );
					return;
				}
			case "hasclose":
				{
					HasClose = bool.Parse( value );
					return;
				}


			case "width":
				{
					Style.Width = Length.Parse( value );
					return;
				}
			case "height":
				{
					Style.Height = Length.Parse( value );
					return;
				}


			case "x":
				{
					Position.x = Length.Parse( value ).Value.Value;
					return;
				}
			case "y":
				{
					Position.y = Length.Parse( value ).Value.Value;
					return;
				}


			case "minwidth":
				{
					MinSize.x = Length.Parse( value ).Value.Value;
					return;
				}
			case "minheight":
				{
					MinSize.y = Length.Parse( value ).Value.Value;
					return;
				}
		}
	}
}
