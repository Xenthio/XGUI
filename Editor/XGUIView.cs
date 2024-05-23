using Editor;
using Sandbox;
using Sandbox.UI;

namespace XGUI;

public class XGUIView : NativeRenderingWidget
{
	RootPanel rootpanel;
	XGUIRootPanel panel;
	SceneModel model;

	SceneWorld world;

	public Window Window;

	public XGUIView()
	{
		MinimumSize = 300;

		world = new SceneWorld();
		model = new SceneModel( world, "models/dev/gordon_at_desk.vmdl", Transform.Zero );
		Camera = new SceneCamera( "XGUIRenderView" );
		Camera.World = world;
		Camera.BackgroundColor = Color.Black;

		rootpanel = new RootPanel();
		rootpanel.Scene = Scene;
		panel = new XGUIRootPanel();
		rootpanel.AddChild( panel );

		Window = new Window();
		Window.StyleSheet.Load( "/XGUI/DefaultStyles/OliveGreen.scss" );
		panel.AddChild( Window );


		new SceneLight( world, new Vector3( 100, 100, 100 ), 500, Color.Orange * 3 );
		new SceneLight( world, new Vector3( -100, -100, 100 ), 500, Color.Cyan * 3 );

		Window.Size = new Vector2( 800, 600 );

	}
	Vector2 lastPos;
	float spinVelocity;
	float hoverTime;
	public override void PreFrame()
	{
		var dir = new Vector3( 0.5f, 0.5f, 0.5f ).Normal;

		var distance = MathX.SphereCameraDistance( model.Bounds.Size.Length * 0.5f, Camera.FieldOfView );
		var aspect = Size.x / Size.y;
		if ( aspect > 1 ) distance *= aspect;

		Camera.World.AmbientLightColor = Color.Gray;
		Camera.Position = model.Bounds.Center + dir * distance * 0.9f;
		Camera.Rotation = Rotation.LookAt( dir * -1.0f, Vector3.Up );
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
