using Sandbox.UI;
public class Resizer : Panel
{
	public Resizer()
	{

		AddClass( "Resizer" );
	}
	bool dragging;
	protected override void OnMouseDown( MousePanelEvent e )
	{
		base.OnMouseDown( e );

		dragging = true;
		xoff = (float)((FindRootPanel().MousePosition.x) - Parent.Box.Rect.Right);
		yoff = (float)((FindRootPanel().MousePosition.y) - Parent.Box.Rect.Bottom);
	}
	protected override void OnMouseUp( MousePanelEvent e )
	{
		base.OnMouseUp( e );
		dragging = false;
	}
	float xoff = 0;
	float yoff = 0;
	protected override void OnMouseMove( MousePanelEvent e )
	{
		base.OnMouseMove( e );
		if ( dragging )
		{
			Parent.Style.Width = (FindRootPanel().MousePosition.x - Parent.Box.Rect.Left) - xoff;
			Parent.Style.Height = (FindRootPanel().MousePosition.y - Parent.Box.Rect.Top) - yoff;
		}
	}
}
