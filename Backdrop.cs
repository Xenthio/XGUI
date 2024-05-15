using Sandbox.UI;
namespace XGUI;

public partial class Backdrop : Panel
{
	public Backdrop()
	{

		AddClass( "Panel" );
		AddClass( "Backdrop" );

		Style.PointerEvents = PointerEvents.All;
		Style.Position = PositionMode.Absolute;
		Style.FlexDirection = FlexDirection.Column;
		Style.Width = Length.Percent( 100 );
		Style.Height = Length.Percent( 100 );
	}

}
