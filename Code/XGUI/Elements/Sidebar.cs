using Sandbox.UI;
namespace XGUI;
public class Sidebar : TabContainer
{
	public Sidebar()
	{
		RemoveClass( "TabContainer" );
		AddClass( "Sidebar" );
	}
}
