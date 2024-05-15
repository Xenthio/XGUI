using System.Linq;

namespace Sandbox.UI
{
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
			SetClass( "focus", HasFocus || Children.Where( x => x.HasFocus ).Any() || Children.OfType<Selector>().Where( x => x.IsOpen ).Any() );
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
}
