using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Linq;
namespace XGUI;

public partial class Pane : Panel
{
	/// <summary>
	/// Which panel triggered this popup. Set by <see cref="SetPositioning"/> or the constructor.
	/// </summary>
	public Panel PopupSource { get; set; }

	/// <summary>
	/// Currently selected option in the popup. Used internally for keyboard navigation.
	/// </summary>
	public Panel SelectedChild { get; set; }

	/// <summary>
	/// Positioning mode for this popup.
	/// </summary>
	public PositionMode Position { get; set; }

	/// <summary>
	/// Offset away from <see cref="PopupSource"/> based on <see cref="Position"/>.
	/// </summary>
	public float PopupSourceOffset { get; set; }

	/// <summary>
	/// Dictates where a <see cref="Popup"/> is positioned.
	/// </summary>
	public enum PositionMode
	{
		/// <summary>
		/// To the left of the source panel, centered.
		/// </summary>
		Left,

		/// <summary>
		/// To the left of the source panel, aligned to the bottom.
		/// </summary>
		LeftBottom,

		/// <summary>
		/// Above the source panel, aligned to the left.
		/// </summary>
		AboveLeft,

		/// <summary>
		/// Below the source panel, aliging on the left. Do not stretch to size of <see cref="Popup.PopupSource"/>.
		/// </summary>
		BelowLeft,

		/// <summary>
		/// Below the source panel, centered horizontally.
		/// </summary>
		BelowCenter,

		/// <summary>
		/// Below the source panel, stretch to the width of the <see cref="Popup.PopupSource"/>.
		/// </summary>
		BelowStretch
	}

	public Pane()
	{

	}

	/// <inheritdoc cref="SetPositioning"/>
	public Pane( Panel sourcePanel, PositionMode position, float offset )
	{
		SetPositioning( sourcePanel, position, offset );
		this.AddEventListener( "onmousedown", MouseDown );
		AcceptsFocus = true;
		Focus();
	}

	/// <summary>
	/// Sets <see cref="PopupSource"/>, <see cref="Position"/> and <see cref="PopupSourceOffset"/>.
	/// Applies relevant CSS classes.
	/// </summary>
	/// <param name="sourcePanel">Which panel triggered this popup.</param>
	/// <param name="position">Desired positioning mode.</param>
	/// <param name="offset">Offset away from the <paramref name="sourcePanel"/>.</param>
	public void SetPositioning( Panel sourcePanel, PositionMode position, float offset )
	{
		Parent = sourcePanel.AncestorsAndSelf.OfType<Window>().FirstOrDefault();
		PopupSource = sourcePanel;
		Position = position;
		PopupSourceOffset = offset;

		AddClass( "pane-panel" );
		PositionMe();

		switch ( Position )
		{
			case PositionMode.Left:
				AddClass( "left" );
				break;

			case PositionMode.LeftBottom:
				AddClass( "left-bottom" );
				break;

			case PositionMode.AboveLeft:
				AddClass( "above-left" );
				break;

			case PositionMode.BelowLeft:
				AddClass( "below-left" );
				break;

			case PositionMode.BelowCenter:
				AddClass( "below-center" );
				break;

			case PositionMode.BelowStretch:
				AddClass( "below-stretch" );
				break;
		}
	}



	/// <summary>
	/// Header panel that holds <see cref="TitleLabel"/> and <see cref="IconPanel"/>.
	/// </summary>
	protected Panel Header;

	/// <summary>
	/// Label that dispalys <see cref="Title"/>.
	/// </summary>
	protected Label TitleLabel;

	/// <summary>
	/// Panel that dispalys <see cref="Icon"/>.
	/// </summary>
	protected IconPanel IconPanel;

	void CreateHeader()
	{
		if ( Header != null ) return;

		Header = Add.Panel( "header" );

		IconPanel = Header.Add.Icon( null );
		TitleLabel = Header.Add.Label( null, "title" );
	}

	/// <summary>
	/// If set, will add an unselectable header with given text and <see cref="Icon"/>.
	/// </summary>
	public string Title
	{
		get => TitleLabel?.Text;
		set
		{
			CreateHeader();
			TitleLabel.Text = value;
		}
	}

	/// <summary>
	/// If set, will add an unselectable header with given icon and <see cref="Title"/>.
	/// </summary>
	public string Icon
	{
		get => IconPanel?.Text;
		set
		{
			CreateHeader();
			IconPanel.Text = value;
		}
	}

	/// <summary>
	/// Closes all panels, marks this one as a success and closes it.
	/// </summary>
	public void Success()
	{
		AddClass( "success" );
		Popup.CloseAll();
	}

	/// <summary>
	/// Closes all panels, marks this one as a failure and closes it.
	/// </summary>
	public void Failure()
	{
		AddClass( "failure" );
		Popup.CloseAll();
	}

	/// <summary>
	/// Add an option to this popup with given text and click action.
	/// </summary>
	public Panel AddOption( string text, Action action = null )
	{
		return Add.Button( text, () =>
		{
			action?.Invoke();
		} );
	}

	/// <summary>
	/// Add an option to this popup with given text, icon and click action.
	/// </summary>
	public Panel AddOption( string text, string icon, Action action = null )
	{
		return Add.ButtonWithIcon( text, icon, null, () =>
		{
			action?.Invoke();
		} );
	}

	/// <summary>
	/// Move selection in given direction.
	/// </summary>
	/// <param name="dir">Positive numbers move selection downwards, negative - upwards.</param>
	public void MoveSelection( int dir )
	{
		var currentIndex = GetChildIndex( SelectedChild );

		if ( currentIndex >= 0 ) currentIndex += dir;
		else if ( currentIndex < 0 ) currentIndex = dir == 1 ? 0 : -1;

		SelectedChild?.SetClass( "active", false );
		SelectedChild = GetChild( currentIndex, true );
		SelectedChild?.SetClass( "active", true );
	}

	public void MouseDown()
	{
	}

	public override void Tick()
	{
		base.Tick();
		if ( !(HasFocus || PopupSource.HasFocus) )
		{
			if ( PopupSource is Selector dp )
			{
				dp.Close();
			}
		}
		PositionMe();

	}

	void PositionMe()
	{
		var rect = PopupSource.Box.Rect;

		var w = Parent.Box.Rect.Width * PopupSource.ScaleFromScreen;
		var h = Parent.Box.Rect.Height * PopupSource.ScaleFromScreen;


		Style.MaxHeight = Screen.Height - 50;

		Style.Width = rect.Width;
		Style.Left = (rect.Left - Parent.Box.Left - 1);
		Style.Top = (rect.Bottom - Parent.Box.Top - 1) + PopupSourceOffset;

		Style.Dirty();
	}

}
