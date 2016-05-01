using System;
using spaar.ModLoader;
using UnityEngine;

namespace HUD_Mod
{

    // If you need documentation about any of these values or the mod loader
    // in general, take a look at https://spaar.github.io/besiege-modloader.

    public class YourMod : Mod
    {
        public override Version Version { get { return new Version("0.3"); } }
        public override string Name { get { return "HUD_Mod"; } }
        public override string DisplayName { get { return "HUD Mod"; } }
        public override string BesiegeVersion { get { return "v0.27"; } }
        public override string Author { get { return "覅是"; } }
        public GameObject temp;

        public override void OnLoad()
        {
            temp = new GameObject();
            temp.name = "HUD_Mod";
            temp.AddComponent<HUDthingy>();
            GameObject.DontDestroyOnLoad(temp);
        }

        public override void OnUnload()
        {
            GameObject.Destroy(temp);
            // Your code here
            // e.g. save configuration, destroy your objects if CanBeUnloaded is true etc
        }
    }
    public class HUDthingy : MonoBehaviour
    {
        public Texture 校准纹理;
        private Texture 俯仰刻度;
        private Texture 机体准星;
        private Texture 罗盘纹理;
        private Texture 正00纹理;
        private Texture 负00纹理;
        private Texture 冰层纹理;
        private Texture 地面那一条杠杠滴纹理;
        private Texture 现时高度指示纹理;

        private GUITexture G俯仰刻度;
        public bool 高度计_渐变中 = false;
        public int 高度计状态 = 0; //-1 地底   0 冰层下   1 1000下   2 1000上
        public int 比较用高度计状态 = 0; //-1 地底   0 冰层下   1 1000下   2 1000上
        public Vector2 pos = new Vector2(Screen.width / 2, Screen.height / 2);
        public float 调试函数 = 0;
        private Key LCF1 = new Key(KeyCode.LeftControl, KeyCode.F2);
        private Key RCF1 = new Key(KeyCode.RightControl, KeyCode.F2);
        public bool 强制关闭 = false;


        private float LastFUveloMag;


        void Start()
        {
            //水平纹理 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/HorizHUD.png").texture;
            WWW w = new WWW("File:///" + Application.dataPath + "/Mods/Resources/校准.png");
            WWW 准星 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/机体准星.png");
            WWW 水平 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/俯仰刻度1.png");
            WWW 罗盘 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/罗盘指针.png");
            WWW 正0 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/零零正面.png");
            WWW 负0 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/零零背面.png");
            WWW 冰层 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Ice Floor.PNG");
            WWW 杠杠 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Floor Line.PNG");
            WWW 现高 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Height Line.PNG");
            /*yield return w;
            yield return 准星;
            yield return 水平;
            yield return 罗盘;*/
            校准纹理 = w.texture;
            机体准星 = 准星.texture;
            俯仰刻度 = 水平.texture;
            罗盘纹理 = 罗盘.texture;
            正00纹理 = 正0.texture;
            负00纹理 = 负0.texture;
            冰层纹理 = 冰层.texture;
            地面那一条杠杠滴纹理 = 杠杠.texture;
            现时高度指示纹理 = 现高.texture;
            G俯仰刻度.texture = 俯仰刻度;
            //水平球.GetComponent<MeshRenderer>().material = new Material(Shader.Find("FX/Glass/Stained BumpDistort"));
            DontDestroyOnLoad(校准纹理);
            DontDestroyOnLoad(俯仰刻度);
            DontDestroyOnLoad(机体准星);
            DontDestroyOnLoad(罗盘纹理);
            DontDestroyOnLoad(正00纹理);
            DontDestroyOnLoad(负00纹理);
            DontDestroyOnLoad(冰层纹理);
            DontDestroyOnLoad(地面那一条杠杠滴纹理);
            DontDestroyOnLoad(现时高度指示纹理);
            DontDestroyOnLoad(G俯仰刻度);
        }
        void FixedUpdate()
        {
            this.transform.localScale = Vector3.zero;
            /*if (GameObject.Find("Main Camera").GetComponent<MouseOrbit>().target.GetComponent<Rigidbody>())
            {
                float mag = GameObject.Find("Main Camera").GetComponent<MouseOrbit>().target.GetComponent<Rigidbody>().velocity.magnitude;
                float ACC = Math.Abs(mag - LastFUveloMag);
                GameObject.Find("Main Camera").GetComponent<MouseOrbit>().focusLerpSmooth = mag+ACC * 2 + 25;
                LastFUveloMag = mag;
            }
            else if(GameObject.Find("Main Camera").GetComponent<MouseOrbit>().target.name == ("CamFollow") && AddPiece.isSimulating)
            {
                float mag = GameObject.Find("Simulation Machine").transform.Find("StartingBlock").GetComponent<Rigidbody>().velocity.magnitude;
                float ACC = Math.Abs(mag - LastFUveloMag);
                GameObject.Find("Main Camera").GetComponent<MouseOrbit>().focusLerpSmooth = mag + (ACC + 1) * 2 + 25;
                LastFUveloMag = mag;
            }
            else
            {
                GameObject.Find("Main Camera").GetComponent<MouseOrbit>().focusLerpSmooth = 25;
            }*/


            if (AddPiece.isSimulating)
            {
                if (GameObject.Find("Main Camera").GetComponent<MouseOrbit>().target.transform.IsChildOf(Machine.Active().SimulationMachine) ^ GameObject.Find("Main Camera").GetComponent<MouseOrbit>().target.transform.IsChildOf(Machine.Active().BuildingMachine))
                {
                    float mag = GameObject.Find("Main Camera").GetComponent<MouseOrbit>().target.GetComponent<Rigidbody>().velocity.magnitude;
                    float ACC = Math.Abs(mag - LastFUveloMag);
                    GameObject.Find("Main Camera").GetComponent<MouseOrbit>().focusLerpSmooth = Mathf.Infinity;
                    LastFUveloMag = mag;
                }
                else if (GameObject.Find("Main Camera").GetComponent<MouseOrbit>().target.name == ("CamFollow") && AddPiece.isSimulating)
                {
                    float mag = GameObject.Find("Simulation Machine").transform.Find("StartingBlock").GetComponent<Rigidbody>().velocity.magnitude;
                    float ACC = Math.Abs(mag - LastFUveloMag);
                    GameObject.Find("Main Camera").GetComponent<MouseOrbit>().focusLerpSmooth = Mathf.Infinity;
                    LastFUveloMag = mag;
                }
                else
                {
                    GameObject.Find("Main Camera").GetComponent<MouseOrbit>().focusLerpSmooth = 8;
                }
            }


        }
        void OnGUI()
        {
            //if (GameObject.Find("Main Camera").GetComponent<MouseOrbit>().isActive ^ 强制关闭) return;
            float 全局屏幕比值W = Screen.width / 1920;
            float 全局屏幕比值H = Screen.height / 1080;
            Matrix4x4 UnRotatedTempMatrix = GUI.matrix;
            Transform MainCameraTransform = GameObject.Find("Main Camera").transform;
            Vector3 zerooncamera = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(Vector3.zero);
            GUIUtility.RotateAroundPivot(MainCameraTransform.eulerAngles.z, new Vector2(zerooncamera.x - 20, (Screen.height - zerooncamera.y) - 20));
            if (zerooncamera.z > 0)
            {
                GUI.DrawTexture(
                    new Rect(
                        new Vector2(
                            zerooncamera.x - 20,
                            (Screen.height - zerooncamera.y) - 20),
                        new Vector2(40, 40)),
                    正00纹理);
            }
            else if (zerooncamera.z < 0)
            {
                GUI.DrawTexture(
                    new Rect(
                        new Vector2(
                            zerooncamera.x - 20,
                            (Screen.height - zerooncamera.y) - 20),
                        new Vector2(40, 40)),
                    负00纹理);
            }
            GUI.matrix = UnRotatedTempMatrix;
            //Camera.main.gameObject.AddComponent<HUDthingy>();
            //水平球.transform.position = Camera.main.gameObject.transform.position;
            //罗盘球.transform.position = Camera.main.gameObject.transform.position;
            //指示球.transform.position = Camera.main.gameObject.transform.position;
            //GUI.DrawTexture(new Rect(new Vector2(40, 40), new Vector2(40, 40)), 校准纹理, ScaleMode.ScaleAndCrop);
            //GUIUtility.ScaleAroundPivot(new Vector2(Screen.width / 1920, Screen.height / 1080), new Vector2(Screen.width, Screen.height) / 2);
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 130 / 2, Screen.height / 2 - 7), new Vector2(130, 46)), 机体准星);
            GUIUtility.RotateAroundPivot(MainCameraTransform.eulerAngles.z, new Vector2(Screen.width / 2, Screen.height / 2));
            float FOVHeight = 180 / GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView;
            float Height = MainCameraTransform.position.y;
            float 视角 = MainCameraTransform.eulerAngles.x;
            float 方向 = MainCameraTransform.eulerAngles.y;

            if (视角 >= 270) 视角 -= 360;
            Matrix4x4 RotatedTempMatrix = GUI.matrix;
            //G俯仰刻度.pixelInset = (new Rect(new Vector2(0 - 453.5f / 2, 0 - (1215 * 1f) * ((视角 + 180 - 10f) / 180)),
            //new Vector2(453.5f, 1215 * 1)));

            GUI.DrawTexture(
                new Rect(new Vector2(Screen.width / 2 - 453.5f / 2, Screen.height - (1215 * 1f) * ((视角 + 180 - 10f * 全局屏幕比值W) / 180)),
                new Vector2(453.5f, 1215 * 1))
                , 俯仰刻度
                , ScaleMode.ScaleAndCrop);


            绘制罗盘(方向, 罗盘纹理, RotatedTempMatrix);
            绘制罗盘(方向 + 90, 罗盘纹理, RotatedTempMatrix);
            绘制罗盘(方向 - 90, 罗盘纹理, RotatedTempMatrix);
            绘制罗盘(方向 + 180, 罗盘纹理, RotatedTempMatrix);
            绘制罗盘(方向 + 180, 罗盘纹理, RotatedTempMatrix);

            高度计状态 = 判断高度计状态(MainCameraTransform.position.y);

            if (高度计状态 != 比较用高度计状态)
            {
                高度计_渐变中 = true;
            }
            else if (高度计_渐变中 == false)
            {
                比较用高度计状态 = 高度计状态;
            }

            if (高度计_渐变中)
            {
                绘制渐变高度计(MainCameraTransform.position.y, 高度计状态);
            }
            if (高度计状态 == -1) { 绘制天花板高度计(MainCameraTransform.position.y); }
            if (高度计状态 == 0) { 绘制下冰层高度计(MainCameraTransform.position.y); }
            if (高度计状态 == 1) { 绘制下千高度计(MainCameraTransform.position.y); }
            if (高度计状态 == 2) { 绘制随意高度计(MainCameraTransform.position.y); }
        }
        void Update()
        {
            if (LCF1.Pressed() ^ RCF1.Pressed())
            {
                强制关闭 = !强制关闭;
            }
        }
        void SuperDebug()
        {
            if (Input.GetKey(KeyCode.UpArrow)) pos += Vector2.up / 5;
            if (Input.GetKey(KeyCode.DownArrow)) pos += Vector2.down / 5;
            if (Input.GetKey(KeyCode.LeftArrow)) pos += Vector2.left / 5;
            if (Input.GetKey(KeyCode.RightArrow)) pos += Vector2.right / 5;
            if (Input.GetKey(KeyCode.K)) Debug.Log(pos + " and " + Screen.width + " and " + Screen.height);
        }

        void 绘制罗盘(float 输入方向, Texture 纹理, Matrix4x4 正常矩阵)
        {
            GUIUtility.RotateAroundPivot(
            -输入方向,
            new Vector2(
            Screen.width / 2 + Screen.width * (Mathf.Sin(-输入方向 * Mathf.Deg2Rad)) - 2.5f,
            Screen.height / 2 + Screen.height / 4 + (Screen.height / 2) * Math.Abs(Mathf.Sin(输入方向 / 2 * Mathf.Deg2Rad))
            ));
            GUI.DrawTexture(
            new Rect(
            new Vector2(
                Screen.width / 2 + Screen.width * (Mathf.Sin(-输入方向 * Mathf.Deg2Rad)) - 2.5f,
                Screen.height / 2 + Screen.height / 4 + (Screen.height / 2) * Math.Abs(Mathf.Sin(输入方向 / 2 * Mathf.Deg2Rad))),
            new Vector2(5, 40)),
            纹理);
            GUI.matrix = 正常矩阵;
        }
        void 绘制下冰层高度计(float CurrentHeight)
        {
            Transform ICEtrans = GameObject.Find("ICE FREEZE").transform;
            float IceFreezehickness = ICEtrans.localScale.y;
            float IceCenterHeight = ICEtrans.position.y;
            float IF比 = (49.5f / IceFreezehickness);
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比, Screen.height / 1080 * 40 * IF比), new Vector2(283.5f * IF比, 100 * IF比)), 冰层纹理);
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比, Screen.height / 1080 * 40 * IF比 + (100 * IF比 / IceFreezehickness * IceCenterHeight)), new Vector2(283.5f * IF比, 10)), 地面那一条杠杠滴纹理);
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比, Screen.height / 1080 * 40 * IF比 + (100 * IF比 / IceFreezehickness * (IceCenterHeight - CurrentHeight))), new Vector2(283.5f * IF比, 10)), 现时高度指示纹理);
        }
        void 绘制下千高度计(float CurrentHeight)
        {
            Transform ICEtrans = GameObject.Find("ICE FREEZE").transform;
            float IceFreezehickness = ICEtrans.localScale.y;
            float IceCenterHeight = ICEtrans.position.y;
            float IF比 = (49.5f / IceFreezehickness);
            float 千比 = (800 * IF比 - 20 * IF比) / 1000;
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 800 * IF比), new Vector2(283.5f * IF比 * 千比, 5 * IF比)), 地面那一条杠杠滴纹理);
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 20 * IF比), new Vector2(283.5f * IF比 * 千比, 5 * IF比)), 地面那一条杠杠滴纹理);
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 800 * IF比 - 千比 * IceCenterHeight), new Vector2(283.5f * IF比 * 千比, 100 * IF比 * 千比)), 冰层纹理);
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 800 * IF比 - 千比 * CurrentHeight), new Vector2(283.5f * IF比 * 千比, 10)), 现时高度指示纹理);
        }
        void 绘制随意高度计(float CurrentHeight)
        {
            Transform ICEtrans = GameObject.Find("ICE FREEZE").transform;
            float IceFreezehickness = ICEtrans.localScale.y;
            float IceCenterHeight = ICEtrans.position.y;
            float IF比 = (49.5f / IceFreezehickness);
            float 自比 = (800 * IF比 - 20 * IF比) / CurrentHeight;
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 自比, Screen.height / 1080 * 20 * IF比), new Vector2(283.5f * IF比 * 自比, 10)), 现时高度指示纹理);
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 自比, Screen.height / 1080 * 800 * IF比), new Vector2(283.5f * IF比 * 自比, 5 * IF比)), 地面那一条杠杠滴纹理);
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 自比, Screen.height / 1080 * 800 * IF比 - 自比 * 1000), new Vector2(283.5f * IF比 * 自比, 5 * IF比)), 地面那一条杠杠滴纹理);
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 自比, Screen.height / 1080 * 800 * IF比 - 自比 * IceCenterHeight), new Vector2(283.5f * IF比 * 自比, 100 * IF比 * 自比)), 冰层纹理);

        }
        void 绘制天花板高度计(float CurrentHeight)
        {
            Transform ICEtrans = GameObject.Find("ICE FREEZE").transform;
            float IceFreezehickness = ICEtrans.localScale.y;
            float IceCenterHeight = ICEtrans.position.y;

            float IF比 = (49.5f / IceFreezehickness);
            float IF2比 = IF比 * (IceCenterHeight/(IceCenterHeight - CurrentHeight));
            float WidthScale = IF比 * IceCenterHeight / (IceCenterHeight - CurrentHeight);

            GUI.DrawTexture(
                new Rect(
                    new Vector2(
                        Screen.width / 2 - 100 * IF比 - 283.5f * WidthScale,
                        Screen.height / 1080 * 40 * IF比), 
                    new Vector2(
                        283.5f * WidthScale, 
                        100 * IF2比)), 
                冰层纹理);

            GUI.DrawTexture(
                new Rect(
                    new Vector2(
                        Screen.width / 2 - 100 * IF比 - 283.5f * WidthScale, 
                        Screen.height / 1080 * 40 * IF比 + (100 * IF比 / IceFreezehickness * IceCenterHeight)), 
                    new Vector2(
                        283.5f * WidthScale, 
                        10)), 
                现时高度指示纹理); 




            GUI.DrawTexture(
                new Rect(
                    new Vector2(
                        Screen.width / 2 - 100 * IF比 - 283.5f * WidthScale,
                        Screen.height/1080 * 40 * IF比 + (IceCenterHeight / (IceCenterHeight - CurrentHeight)) * (100 * IF比 / IceFreezehickness * IceCenterHeight)), 
                    new Vector2(
                        283.5f * WidthScale,
                        10)), 
                地面那一条杠杠滴纹理);
        }
        void 绘制渐变高度计(float CurrentHeight, int ToSituation)
        {
            高度计_渐变中 = false;
        }
        int 判断高度计状态(float Height)
        {
            int 最终状态;
            if (GameObject.Find("ICE FREEZE"))
            {
                Transform ICEtrans = GameObject.Find("ICE FREEZE").transform;
                float IceFreezehickness = ICEtrans.localScale.y;
                float IceCenterHeight = ICEtrans.position.y;
                if (Height < 0)
                {
                    最终状态 = -1;
                }
                else if (Height < (IceCenterHeight - IceFreezehickness / 2))
                {
                    最终状态 = 0;
                }
                else if (Height < 1000)
                {
                    最终状态 = 1;
                }
                else
                {
                    最终状态 = 2;
                }
            }
            else
            {
                if (Height <= 0)
                {
                    最终状态 = -1;
                }
                else if (Height < 1000)
                {
                    最终状态 = 1;
                }
                else
                {
                    最终状态 = 2;
                }
            }
            return 最终状态;

        }
    }
}
