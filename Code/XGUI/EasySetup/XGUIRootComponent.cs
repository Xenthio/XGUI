using Sandbox;
namespace XGUI;
public class XGUIRootComponent : PanelComponent
{
	protected override void OnStart()
	{
		base.OnStart();
		var pnl = new XGUIRootPanel();
		Panel.AddChild( pnl );
	}
}
