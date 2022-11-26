using FairyGUI;
using GameManager;
using Microsoft.Xna.Framework.Graphics;

namespace WorldOfTheThreeKingdoms.GameScreens.ScreenLayers
{
	public class BagScene : GComponent
	{
		GComponent _mainView;
		BagWindow _bagWindow;

		public BagScene()
		{
			UIPackage.AddPackage("UI/Bag");
			UIObjectFactory.SetLoaderExtension(typeof(MyGLoader));
			Texture2D t= CacheManager.LoadTexture("Content/UI/Bag_atlas0.png");
			_mainView = UIPackage.CreateObject("Bag", "Main", t, null).asCom;
            //_mainView = UIPackage.CreateObject("Bag", "Main", t, null).asCom;
            _mainView.MakeFullScreen();
            _mainView.AddRelation(GRoot.inst, RelationType.Size);
            GRoot.inst.AddChild(_mainView);

            _bagWindow = new BagWindow();
			_mainView.GetChild("bagBtn").onClick.Add(() => { _bagWindow.Show(); });
		}
	}
}