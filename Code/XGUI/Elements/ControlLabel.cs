using Sandbox;
using Sandbox.UI;
using System.Linq;
namespace XGUI;

[Library( "controllabel" )]
public class ControlLabel : Panel
{
	Label Label;
	public ControlLabel()
	{
		AddClass( "controllabel" );
		Label = AddChild<Label>();
	}
	public override void Tick()
	{
		base.Tick();
		SetClass( "focus", PanelHasFocus( this ) || Children.Where( x => PanelHasFocus( x ) ).Any() || Children.OfType<Selector>().Where( x => x.IsOpen ).Any() );
	}
	public bool PanelHasFocus( Panel panel )
	{
		return panel.HasFocus || panel.HasClass( "focus" );
	}
	public override void SetProperty( string name, string value )
	{
		if ( name == "label" )
		{
			Label.Text = value;
			return;
		}
	}
}
