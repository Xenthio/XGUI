using Editor;
using Sandbox;
using Sandbox.UI;

namespace XGUI;

public class XGUIView : NativeRenderingWidget
{
	RootPanel rootpanel;
	XGUIRootPanel panel;

	SceneWorld world;

	public Window Window;

	public XGUIView()
	{
		MinimumSize = 300;

		world = new SceneWorld();
		Camera = new SceneCamera( "XGUIRenderView" );
		Camera.World = world;
		Camera.BackgroundColor = Color.Yellow;

		rootpanel = new RootPanel();
		rootpanel.Scene = Scene;
		panel = new XGUIRootPanel();
		rootpanel.AddChild( panel );

		Window = new Window();
		Window.StyleSheet.Load( "/XGUI/DefaultStyles/OliveGreen.scss" );
		panel.AddChild( Window );

		Window.Size = new Vector2( 800, 600 );

	}
	public override void PreFrame()
	{

		Camera.World.AmbientLightColor = IsUnderMouse ? Color.Gray : Color.Black;
		Camera.ZFar = 10000;
		Camera.FieldOfView = 50;
		Camera.AntiAliasing = false;
	}

	public override void OnDestroyed()
	{
		Window.Delete();
		panel.Delete();
		rootpanel.Delete();
		base.OnDestroyed();
	}
}
