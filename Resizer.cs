using Sandbox.UI;
namespace XGUI;
public class Resizer : Panel
{
	public Resizer()
	{

		AddClass( "Resizer" );
	}

	protected override void OnMouseDown( MousePanelEvent e )
	{
		base.OnMouseDown( e );

		var parent = Parent as Window;
		parent.draggingB = true;
		parent.draggingR = true;
		//draggingT = true;
		//draggingL = true;
		parent.xoff1 = (float)((FindRootPanel().MousePosition.x) - Parent.Box.Rect.Right);
		parent.yoff1 = (float)((FindRootPanel().MousePosition.y) - Parent.Box.Rect.Bottom);
		parent.xoff2 = (float)((FindRootPanel().MousePosition.x) - Parent.Box.Rect.Left);
		parent.yoff2 = (float)((FindRootPanel().MousePosition.y) - Parent.Box.Rect.Top);
	}
	protected override void OnMouseUp( MousePanelEvent e )
	{
		base.OnMouseUp( e );
		var parent = Parent as Window;
		parent.draggingB = false;
		parent.draggingR = false;
		parent.draggingT = false;
		parent.draggingL = false;
		parent.xoff1 = 0;
		parent.yoff1 = 0;
		parent.xoff2 = 0;
		parent.yoff2 = 0;
	}
	protected override void OnMouseMove( MousePanelEvent e )
	{
		base.OnMouseMove( e );

		var parent = Parent as Window;

		parent.ResizeMove();
	}
}
