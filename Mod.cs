using System;
using spaar.ModLoader;
using UnityEngine;
using System.Collections;

namespace HUD_Mod
{

    // If you need documentation about any of these values or the mod loader
    // in general, take a look at https://spaar.github.io/besiege-modloader.

    public class HUD_MOD : Mod
    {
        public override Version Version { get { return new Version("1.01"); } }
        public override string Name { get { return "HUD_Mod"; } }
        public override string DisplayName { get { return "HUD Mod"; } }
        public override string BesiegeVersion { get { return "v0.32"; } }
        public override string Author { get { return "覅是"; } }
        public override bool CanBeUnloaded { get { return true; } }
        public GameObject temp;


        public override void OnLoad()
        {
            temp = new GameObject();
            temp.name = "HUD Mod";
            temp.AddComponent<HUDthingy>();
            GameObject.DontDestroyOnLoad(temp);
        }

        public override void OnUnload()
        {
            GameObject.Destroy(temp.GetComponent<HUDthingy>());
            GameObject.Destroy(temp);
        }
    }
    public class HUDthingy : MonoBehaviour
    {
        //public Texture 校准纹理;
        private Texture 俯仰刻度;
        private Texture 机体准星;
        private Texture 罗盘纹理;
        private Texture 正00纹理;
        private Texture 负00纹理;
        private Texture 冰层纹理;
        private Texture 地面那一条杠杠滴纹理;
        private Texture 现时高度指示纹理;
        private Texture 一千杠杠;
        private Texture 速度标识轨迹线;
        private Texture 速度标识;

        public bool 俯仰刻度On = true;
        public bool 罗盘纹理On = true;
        public bool 机体准星On = true;
        public bool 正0纹理On = true;
        public bool 负0纹理On = true;
        public bool 冰层纹理On = true;
        public bool 地面纹理On = true;
        public bool 高度纹理On = true;
        public bool 一千纹理On = true;
        public bool 速度标识纹理On = true;


        //private GameObject 冲刺效果 = GameObject.CreatePrimitive(PrimitiveType.Plane);

        public bool 高度计_渐变中 = false;
        public int 高度计状态 = 0; //-1 地底   0 冰层下   1 1000下   2 1000上
        public int 比较用高度计状态 = 0; //-1 地底   0 冰层下   1 1000下   2 1000上
        public Vector2 pos = new Vector2(Screen.width / 2, Screen.height / 2);
        public float 调试函数 = 0;
        private Key LCF1 = new Key(KeyCode.LeftControl, KeyCode.F2);
        private Key RCF1 = new Key(KeyCode.RightControl, KeyCode.F2);
        private float CurrentCameraSpeed;
        private float 速度标记位置 = 0;
        public bool 强制关闭 = false;
        public float 渐变高度计使用的临时函数 = 0;


        private float LastFUveloMag = 0;

        IEnumerator ApplyAllTextures()
        {
            //水平纹理 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/HorizHUD.png").texture;
            //冲刺效果 = new GameObject();
            //冲刺效果.name = "冲刺效果";
            //冲刺效果.transform.SetParent(this.transform);
            //冲刺效果.transform.position = Vector3.zero;
            //WWW w = new WWW("File:///" + Application.dataPath + "/Mods/Resources/校准.png");
            //yield return w;
            WWW 准星 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Center.png");
            yield return 准星;
            WWW 罗盘 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Direction Indicator.png");
            yield return 罗盘;
            WWW 正0 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Zero Zero Front.png");
            yield return 正0;
            WWW 负0 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Zero Zero Back.png");
            yield return 负0;
            WWW 冰层 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Ice Floor.PNG");
            yield return 冰层;
            WWW 杠杠 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Floor Line.PNG");
            yield return 杠杠;
            WWW 现高 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Height Line.PNG");
            yield return 现高;
            WWW 千杠 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/OverICE Line.PNG");
            yield return 千杠;
            WWW 斜杠 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Speed Trend.PNG");
            yield return 斜杠;
            WWW 速度 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Speed Sign.PNG");
            yield return 速度;
            WWW 水平 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Gradienter.png");
            new WaitForEndOfFrame();
            yield return 水平;
            //WWW 速度Bump = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Speed_Bump.PNG");
            //WWW 速度 = new WWW("File:///" + Application.dataPath + "/Mods/Resources/Speed.PNG");
            /*yield return w;
            yield return 准星;
            yield return 水平;
            yield return 罗盘;*/
            //校准纹理 = w.texture;
            机体准星 = 准星.texture;
            俯仰刻度 = 水平.texture;
            罗盘纹理 = 罗盘.texture;
            正00纹理 = 正0.texture;
            负00纹理 = 负0.texture;
            冰层纹理 = 冰层.texture;
            地面那一条杠杠滴纹理 = 杠杠.texture;
            现时高度指示纹理 = 现高.texture;
            一千杠杠 = 千杠.texture;
            速度标识轨迹线 = 斜杠.texture;
            速度标识 = 速度.texture;
            /*冲刺效果.name = "Speedy";
            冲刺效果.GetComponent<Renderer>().material = new Material(Shader.Find("FX/Glass/Stained BumpDistort"));
            冲刺效果.GetComponent<Renderer>().material.SetTexture("_MainTex", 速度.texture); 
            冲刺效果.GetComponent<Renderer>().material.SetTexture("_BumpMap", 速度Bump.texture);
            //冲刺效果.GetComponent<Renderer>().material.SetColor("_MainTex", Color.blue);
            Destroy(冲刺效果.GetComponent<Collider>());
            //G冲刺效果 = GameObject.Find("Main Camera").gameObject.AddComponent<GUITexture>();
            //G冲刺效果.texture = 速度.texture;
            //水平球.GetComponent<MeshRenderer>().material = new Material(Shader.Find("FX/Glass/Stained BumpDistort"));*/
            //DontDestroyOnLoad(校准纹理);
            //DontDestroyOnLoad(俯仰刻度);
            //DontDestroyOnLoad(机体准星);
            //DontDestroyOnLoad(罗盘纹理);
            //DontDestroyOnLoad(正00纹理);
            //DontDestroyOnLoad(负00纹理);
            //DontDestroyOnLoad(冰层纹理);
            //DontDestroyOnLoad(地面那一条杠杠滴纹理);
            //DontDestroyOnLoad(现时高度指示纹理);
            //DontDestroyOnLoad(一千杠杠);
            //DontDestroyOnLoad(速度标识轨迹线);
            //DontDestroyOnLoad(速度标识);
            //DontDestroyOnLoad(G冲刺效果);
            //DontDestroyOnLoad(冲刺效果);         

        }

        void Start()
        {
            StartCoroutine(ApplyAllTextures());
            try
            {
                LoadSettings();
            }
            catch { SaveSettings(); }
            Commands.RegisterCommand("PitchHUD", (args, notUses) =>
            {
                俯仰刻度On = !俯仰刻度On;
                SaveSettings();
                if (!俯仰刻度On) return "Turned OFF!";
                else return "Turned On!";

            }, "Toggle the Pitch HUD");

            Commands.RegisterCommand("DirectionHUD", (args, notUses) =>
            {
                罗盘纹理On = !罗盘纹理On;
                SaveSettings();
                if (!罗盘纹理On) return "Turned OFF!";
                else return "Turned On!";

            }, "Toggle the Direction HUD");

            Commands.RegisterCommand("CenterHUD", (args, notUses) =>
            {
                机体准星On = !机体准星On;
                SaveSettings();
                if (!机体准星On) return "Turned OFF!";
                else return "Turned On!";

            }, "Toggle the Center HUD");

            Commands.RegisterCommand("FrontZeroHUD", (args, notUses) =>
            {
                正0纹理On = !正0纹理On;
                SaveSettings();
                if (!正0纹理On) return "Turned OFF!";
                else return "Turned On!";

            }, "Toggle the 00 position (front) HUD");

            Commands.RegisterCommand("BackZeroHUD", (args, notUses) =>
            {
                负0纹理On = !负0纹理On;
                SaveSettings();
                if (!负0纹理On) return "Turned OFF!";
                else return "Turned On!";

            }, "Toggle the 00 position (back) HUD");

            Commands.RegisterCommand("IceHUD", (args, notUses) =>
            {
                冰层纹理On = !冰层纹理On;
                SaveSettings();
                if (!冰层纹理On) return "Turned OFF!";
                else return "Turned On!";

            }, "Toggle the HUD that represents Ice Freeze in height system.");

            Commands.RegisterCommand("FloorHUD", (args, notUses) =>
            {
                地面纹理On = !地面纹理On;
                SaveSettings();
                if (!地面纹理On) return "Turned OFF!";
                else return "Turned On!";

            }, "Toggle the HUD that represents floor in height system.");

            Commands.RegisterCommand("1000HUD", (args, notUses) =>
            {
                一千纹理On = !一千纹理On;
                SaveSettings();
                if (!一千纹理On) return "Turned OFF!";
                else return "Turned On!";

            }, "Toggle the HUD that represents 1000 unit height in height system.");

            Commands.RegisterCommand("HeightHUD", (args, notUses) =>
            {
                高度纹理On = !高度纹理On;
                SaveSettings();
                if (!高度纹理On) return "Turned OFF!";
                else return "Turned On!";

            }, "Toggle the HUD that represents camera current height in height system.");

            Commands.RegisterCommand("SpeedHUD", (args, notUses) =>
            {
                速度标识纹理On = !速度标识纹理On;
                SaveSettings();
                if (!速度标识纹理On) return "Turned OFF!";
                else return "Turned On!";

            }, "Toggle the Speed HUD System.");

        }
        void FixedUpdate()
        {
            //this.transform.localScale = Vector3.zero;
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
            //GetComponent<Camera>().ScreenToWorldPoint
            MouseOrbit Mo = GameObject.Find("Main Camera").GetComponent<MouseOrbit>();
            if (!Mo) return;
            if (!Machine.Active().BuildingMachine) return;
            if (Mo.target.IsChildOf(Machine.Active().BuildingMachine))
            {
                float mag = Mo.target.GetComponent<Rigidbody>().velocity.magnitude;
                float ACC = Math.Abs(mag - LastFUveloMag);
                Mo.focusLerpSmooth = Mathf.Infinity;
                Mo.fixedCamLerpPosSpeed = Mathf.Infinity;
                LastFUveloMag = mag;
            }
            else if (StatMaster.isSimulating)
            {
                try
                {
                    if (Mo.target.IsChildOf(Machine.Active().SimulationMachine))
                    {
                        float mag = Mo.target.GetComponent<Rigidbody>().velocity.magnitude;
                        float ACC = Math.Abs(mag - LastFUveloMag);
                        Mo.focusLerpSmooth = Mathf.Infinity;
                        Mo.fixedCamLerpPosSpeed = Mathf.Infinity;
                        LastFUveloMag = mag;
                    }
                }
                catch { }
            }
            else if (Mo.target.name == ("CamFollow") && StatMaster.isSimulating)
            {
                Transform sb = Machine.Active().SimulationMachine.GetComponentInChildren<StartingBlockTag>().transform;
                if (sb != null)
                {
                    float mag = sb.GetComponent<Rigidbody>().velocity.magnitude;
                    float ACC = Math.Abs(mag - LastFUveloMag);
                    Mo.focusLerpSmooth = Mathf.Infinity;
                    Mo.fixedCamLerpPosSpeed = Mathf.Infinity;
                    Mo.target.transform.position = sb.position + sb.GetComponent<Rigidbody>().velocity * Time.fixedDeltaTime;
                    LastFUveloMag = mag;
                }
            }
            else
            {
                Mo.focusLerpSmooth = 8;
                Mo.fixedCamLerpPosSpeed = 8;
            }
            //Transform MCt = GameObject.Find("Main Camera").transform;




        }
        void OnGUI()
        {
            if (!GameObject.Find("Main Camera").GetComponent<MouseOrbit>()) return;
            if (GameObject.Find("Main Camera").GetComponent<MouseOrbit>().isActive || 强制关闭) return;
            float 全局屏幕比值W = Screen.width / 1920;
            float 全局屏幕比值H = Screen.height / 1080;
            Matrix4x4 UnRotatedTempMatrix = GUI.matrix;
            Transform MainCameraTransform = GameObject.Find("Main Camera").transform;
            CurrentCameraSpeed = Vector3.Dot(MainCameraTransform.GetComponent<Camera>().velocity, MainCameraTransform.forward);
            Vector3 zerooncamera = MainCameraTransform.GetComponent<Camera>().WorldToScreenPoint(Vector3.zero);
            GUIUtility.RotateAroundPivot(MainCameraTransform.eulerAngles.z, new Vector2(zerooncamera.x - 20, (Screen.height - zerooncamera.y) - 20));


            if (zerooncamera.z > 0)
            {
                if (正0纹理On)
                {
                    GUI.DrawTexture(
                        new Rect(
                            new Vector2(
                                zerooncamera.x - 20,
                                (Screen.height - zerooncamera.y) - 20),
                            new Vector2(40, 40)),
                        正00纹理);
                }
            }
            else if (zerooncamera.z < 0)
            {
                if (负0纹理On)
                {
                    GUI.DrawTexture(
                    new Rect(
                        new Vector2(
                            zerooncamera.x - 20,
                            (Screen.height - zerooncamera.y) - 20),
                        new Vector2(40, 40)),
                    负00纹理);
                }
            }
            GUI.matrix = UnRotatedTempMatrix;
            //Camera.main.gameObject.AddComponent<HUDthingy>();
            //水平球.transform.position = Camera.main.gameObject.transform.position;
            //罗盘球.transform.position = Camera.main.gameObject.transform.position;
            //指示球.transform.position = Camera.main.gameObject.transform.position;
            //GUI.DrawTexture(new Rect(new Vector2(40, 40), new Vector2(40, 40)), 校准纹理, ScaleMode.ScaleAndCrop);
            //GUIUtility.ScaleAroundPivot(new Vector2(Screen.width / 1920, Screen.height / 1080), new Vector2(Screen.width, Screen.height) / 2);

            if (机体准星On)
            {
                GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 130 / 2, Screen.height / 2 - 7), new Vector2(130, 46)), 机体准星);
            }

            GUIUtility.RotateAroundPivot(MainCameraTransform.eulerAngles.z, new Vector2(Screen.width / 2, Screen.height / 2));
            float FOVHeight = 180 / MainCameraTransform.GetComponent<Camera>().fieldOfView;
            float Height = MainCameraTransform.position.y;
            float 视角 = MainCameraTransform.eulerAngles.x;
            float 方向 = MainCameraTransform.eulerAngles.y;

            if (视角 >= 270) 视角 -= 360;
            Matrix4x4 RotatedTempMatrix = GUI.matrix;
            //G俯仰刻度.pixelInset = (new Rect(new Vector2(0 - 453.5f / 2, 0 - (1215 * 1f) * ((视角 + 180 - 10f) / 180)),
            //new Vector2(453.5f, 1215 * 1)));
            if (俯仰刻度On) {
                GUI.DrawTexture(
                    new Rect(new Vector2(Screen.width / 2 - 453.5f / 2, Screen.height - (1215 * 1f) * ((视角 + 180 - (89.175f - 0.1484868421f * Screen.height / 2)) / 180)),
                    new Vector2(453.5f, 1215 * 1))
                    , 俯仰刻度
                    , ScaleMode.ScaleAndCrop);
            }
            if (罗盘纹理On)
            {

                绘制罗盘(方向, 罗盘纹理, RotatedTempMatrix);
                绘制罗盘(方向 + 90, 罗盘纹理, RotatedTempMatrix);
                绘制罗盘(方向 - 90, 罗盘纹理, RotatedTempMatrix);
                绘制罗盘(方向 + 180, 罗盘纹理, RotatedTempMatrix);
                绘制罗盘(方向 + 180, 罗盘纹理, RotatedTempMatrix);
            }

            if (高度纹理On) { 
            高度计状态 = 判断高度计状态(MainCameraTransform.position.y);

            if (高度计状态 != 比较用高度计状态 && !高度计_渐变中)
            {
                高度计_渐变中 = true;
            }
            if (高度计_渐变中 && (高度计状态 == 1 && 比较用高度计状态 == 0))
            {
                绘制0到1渐变高度计(MainCameraTransform.position.y, 高度计状态);
            }
            else if (高度计_渐变中 && (高度计状态 == 0 && 比较用高度计状态 == 1))
            {
                绘制1到0渐变高度计(MainCameraTransform.position.y, 高度计状态);
            }
            else
            {
                if (高度计状态 == -1) { 绘制天花板高度计(MainCameraTransform.position.y); }
                if (高度计状态 == 0) { 绘制下冰层高度计(MainCameraTransform.position.y); }
                if (高度计状态 == 1) { 绘制下千高度计(MainCameraTransform.position.y); }
                if (高度计状态 == 2) { 绘制随意高度计(MainCameraTransform.position.y); }
            }
        }
            GUI.matrix = UnRotatedTempMatrix;

            //冲刺效果.transform.position = MainCameraTransform.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, (GameObject.Find("Main Camera").GetComponent<Camera>().nearClipPlane + 0.1f /* MainCameraTransform.GetComponent<Camera>().velocity.magnitude*/)));
            //冲刺效果.transform.eulerAngles = new Vector3(MainCameraTransform.eulerAngles.x + 270, MainCameraTransform.eulerAngles.y, MainCameraTransform.eulerAngles.z);
            //冲刺效果.GetComponent<Renderer>().material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
            ////冲刺效果.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
            //冲刺效果.transform.localScale = new Vector3(0.054f, 1, 0.059f);
            if (速度标识纹理On)
            {
                float angel = Mathf.Atan2((Screen.height / 6) * 2, Screen.width / 2) * Mathf.Rad2Deg;
                速度标记位置 += ((CurrentCameraSpeed % 340) / 340) * Time.fixedDeltaTime;
                if (速度标记位置 > 1) { --速度标记位置; }
                if (速度标记位置 < 0) { ++速度标记位置; }
                GUIUtility.RotateAroundPivot(angel, new Vector2(Screen.width / 2, Screen.height / 2));
                GUI.DrawTexture(
                    new Rect(new Vector2(Screen.width / 2, Screen.height / 2),
                    new Vector2(
                        Vector2.Distance(
                            new Vector2(Screen.width / 2, (Screen.height / 6) * 2),
                            Vector2.zero
                            ), 3))
                    , 速度标识轨迹线
                    );
                Rect SpeedRect = new Rect(
                        new Vector2((Vector2.Distance(
                            new Vector2(Screen.width / 2, (Screen.height / 6) * 2),
                            Vector2.zero
                            ) * 速度标记位置) + Screen.width / 2, Screen.height / 2 - (60 * 速度标记位置 + 15) / 2)
                        , new Vector2(60 * 速度标记位置 + 15, 60 * 速度标记位置 + 15));

                GUI.DrawTexture(
                    SpeedRect
                    , 速度标识
                    );
                GUI.Label(
                    new Rect(
                    new Vector2((Vector2.Distance(
                            new Vector2(Screen.width / 2, (Screen.height / 6) * 2),
                            Vector2.zero
                            ) * 速度标记位置) + Screen.width / 2, Screen.height / 2 - (60 * 速度标记位置 + 15) / 2) + new Vector2(-10, 15)
                        , new Vector2(60 * 速度标记位置 + 15, 60 * 速度标记位置 + 15) * 2)
                        , Mathf.RoundToInt(CurrentCameraSpeed).ToString());
            }
        }
        void Update()
        {
            if (LCF1.Pressed() || RCF1.Pressed())
            {
                强制关闭 = !强制关闭;
            }
            MouseOrbit Mo = GameObject.Find("Main Camera").GetComponent<MouseOrbit>();
            if (!Mo) return;
            if (!Machine.Active().BuildingMachine) return;
            try
            {
                if (Mo.target.name == ("CamFollow") && StatMaster.isSimulating)
                {
                    Transform sb = Machine.Active().SimulationMachine.transform.GetComponentInChildren<StartingBlockTag>().transform;
                    if (sb != null)
                    {
                        float mag = sb.GetComponent<Rigidbody>().velocity.magnitude;
                        float ACC = Math.Abs(mag - LastFUveloMag);
                        Mo.focusLerpSmooth = Mathf.Infinity;
                        Mo.target.transform.position = sb.transform.position + sb.GetComponent<Rigidbody>().velocity * Time.fixedDeltaTime;
                        LastFUveloMag = mag;
                    }
                }
            }
            catch { }


            //冲刺效果.transform.parent = GameObject.Find("Main Camera").transform;
        }
        void LateUpdate()
        {
            
        }
        void SuperDebug()
        {
            if (Input.GetKey(KeyCode.UpArrow)) pos += Vector2.up / 5;
            if (Input.GetKey(KeyCode.DownArrow)) pos += Vector2.down / 5;
            if (Input.GetKey(KeyCode.LeftArrow)) pos += Vector2.left / 5;
            if (Input.GetKey(KeyCode.RightArrow)) pos += Vector2.right / 5;
            if (Input.GetKey(KeyCode.K)) Debug.Log(pos + " and " + Screen.width + " and " + Screen.height);
        }
        void LoadSettings()
        {
            俯仰刻度On = Configuration.GetBool("俯仰刻度On", 俯仰刻度On);
            罗盘纹理On = Configuration.GetBool("罗盘纹理On", 罗盘纹理On);
            机体准星On = Configuration.GetBool("机体准星On", 机体准星On);
            正0纹理On = Configuration.GetBool("正0纹理On", 正0纹理On);
            负0纹理On = Configuration.GetBool("负0纹理On", 负0纹理On);
            冰层纹理On = Configuration.GetBool("冰层纹理On", 冰层纹理On);
            地面纹理On = Configuration.GetBool("地面纹理On", 地面纹理On);
            一千纹理On = Configuration.GetBool("一千纹理On", 一千纹理On);
            高度纹理On = Configuration.GetBool("高度纹理On", 高度纹理On);
            速度标识纹理On = Configuration.GetBool("速度标识纹理On", 速度标识纹理On);
        }
        void SaveSettings()
        {
            Configuration.SetBool("俯仰刻度On", 俯仰刻度On);
            Configuration.SetBool("罗盘纹理On", 罗盘纹理On);
            Configuration.SetBool("机体准星On", 机体准星On);
            Configuration.SetBool("正0纹理On", 正0纹理On);
            Configuration.SetBool("负0纹理On", 负0纹理On);
            Configuration.SetBool("冰层纹理On", 冰层纹理On);
            Configuration.SetBool("地面纹理On", 地面纹理On);
            Configuration.SetBool("一千纹理On", 一千纹理On);
            Configuration.SetBool("高度纹理On", 高度纹理On);
            Configuration.SetBool("速度标识纹理On", 速度标识纹理On);
            Configuration.Save();
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
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 800 * IF比), new Vector2(283.5f * IF比 * 千比, 5 * IF比)), 地面那一条杠杠滴纹理);//地面
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 20 * IF比), new Vector2(283.5f * IF比 * 千比, 5 * IF比)), 一千杠杠);//一千
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
            GUI.DrawTexture(new Rect(new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 自比, Screen.height / 1080 * 800 * IF比 - 自比 * 1000), new Vector2(283.5f * IF比 * 自比, 5 * IF比)), 一千杠杠);
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
        void 绘制0到1渐变高度计(float CurrentHeight, int ToSituation)
        {
            Transform ICEtrans = GameObject.Find("ICE FREEZE").transform;
            if (渐变高度计使用的临时函数 == 0) 渐变高度计使用的临时函数 = Time.time;
            float IceFreezehickness = ICEtrans.localScale.y;
            float IceCenterHeight = ICEtrans.position.y;
            float IF比 = (49.5f / IceFreezehickness);
            float 千比 = (800 * IF比 - 20 * IF比) / 1000;
            float zhe = Time.time - 渐变高度计使用的临时函数;
            
            GUI.DrawTexture(
                new Rect(
                    Vector2.Lerp(
                        new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比, Screen.height / 1080 * 800 * IF比),
                        new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 800 * IF比),zhe
                        ),
                    Vector2.Lerp( 
                        new Vector2(283.5f * IF比, 10), 
                        new Vector2(283.5f * IF比 * 千比, 5 * IF比),
                        zhe)
                        ),
                    地面那一条杠杠滴纹理);//地面

            GUI.DrawTexture(
                new Rect(
                    Vector2.Lerp(
                        new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比, Screen.height / 1080 * 40 * IF比),
                        new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 20 * IF比), zhe
                        ),
                    Vector2.Lerp(
                        new Vector2(283.5f * IF比, 10),
                        new Vector2(283.5f * IF比 * 千比, 5 * IF比),
                        zhe)
                        ),
                    一千杠杠);//一千

            GUI.DrawTexture(
                new Rect(
                    Vector2.Lerp(
                        new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比, Screen.height / 1080 * 40 * IF比),
                    new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 800 * IF比 - 千比 * IceCenterHeight),
                    zhe),
                    Vector2.Lerp(
                        new Vector2(283.5f * IF比, 100 * IF比),
                        new Vector2(283.5f * IF比 * 千比, 100 * IF比 * 千比)
                    ,zhe)), 冰层纹理);

            GUI.DrawTexture(
                new Rect(
                    Vector2.Lerp(
                        new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比, Screen.height / 1080 * 40 * IF比 + (100 * IF比 / IceFreezehickness * (IceCenterHeight - CurrentHeight))),
                    new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 800 * IF比 - 千比 * CurrentHeight),zhe), 
                    Vector2.Lerp(
                        new Vector2(283.5f * IF比, 10),
                    new Vector2(283.5f * IF比 * 千比, 10),zhe)), 
                现时高度指示纹理);
            if (Time.time - 渐变高度计使用的临时函数 >= 1 || (ToSituation != 1 && 比较用高度计状态 == 0))
            {
                高度计_渐变中 = false;
                比较用高度计状态 = 高度计状态;
                渐变高度计使用的临时函数 = 0;
            }
        }
        void 绘制1到0渐变高度计(float CurrentHeight, int ToSituation)
        {
            Transform ICEtrans = GameObject.Find("ICE FREEZE").transform;
            if (渐变高度计使用的临时函数 == 0) 渐变高度计使用的临时函数 = Time.time;
            float IceFreezehickness = ICEtrans.localScale.y;
            float IceCenterHeight = ICEtrans.position.y;
            float IF比 = (49.5f / IceFreezehickness);
            float 千比 = (800 * IF比 - 20 * IF比) / 1000;
            float zhe = 1 - (Time.time - 渐变高度计使用的临时函数);
            GUI.DrawTexture(
                new Rect(
                    Vector2.Lerp(
                        new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比, Screen.height / 1080 * 800 * IF比),
                        new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 800 * IF比), zhe
                        ),
                    Vector2.Lerp(
                        new Vector2(283.5f * IF比, 10),
                        new Vector2(283.5f * IF比 * 千比, 5 * IF比),
                        zhe)
                        ),
                    地面那一条杠杠滴纹理);//地面

            GUI.DrawTexture(
                            new Rect(
                                Vector2.Lerp(
                                    new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比, Screen.height / 1080 * 40 * IF比),
                                    new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 20 * IF比), zhe
                                    ),
                                Vector2.Lerp(
                                    new Vector2(283.5f * IF比, 10),
                                    new Vector2(283.5f * IF比 * 千比, 5 * IF比),
                                    zhe)
                                    ),
                                一千杠杠);//一千

            GUI.DrawTexture(
                new Rect(
                    Vector2.Lerp(
                        new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比, Screen.height / 1080 * 40 * IF比),
                    new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 800 * IF比 - 千比 * IceCenterHeight),
                    zhe),
                    Vector2.Lerp(
                        new Vector2(283.5f * IF比, 100 * IF比),
                        new Vector2(283.5f * IF比 * 千比, 100 * IF比 * 千比)
                    , zhe)), 冰层纹理);

            GUI.DrawTexture(
                new Rect(
                    Vector2.Lerp(
                        new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比, Screen.height / 1080 * 40 * IF比 + (100 * IF比 / IceFreezehickness * (IceCenterHeight - CurrentHeight))),
                    new Vector2(Screen.width / 2 - 100 * IF比 - 283.5f * IF比 * 千比, Screen.height / 1080 * 800 * IF比 - 千比 * CurrentHeight), zhe),
                    Vector2.Lerp(
                        new Vector2(283.5f * IF比, 10),
                    new Vector2(283.5f * IF比 * 千比, 10), zhe)),
                现时高度指示纹理);
            if (Time.time - 渐变高度计使用的临时函数 >= 1 || (ToSituation != 0 && 比较用高度计状态 == 1))
            {
                高度计_渐变中 = false;
                比较用高度计状态 = 高度计状态;
                渐变高度计使用的临时函数 = 0;
            }
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
                else if (Height < (IceCenterHeight + IceFreezehickness / 2))
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
                return 最终状态;
            }
            else { return -2; }
            /*else
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
            }*/

        }
    }
    public class 冲刺时的特效:MonoBehaviour
    {
        public GUITexture gt;
        void Start()
        {
            gt = this.GetComponent<GUITexture>();
        }
        void OnGUI()
        {
            gt.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
            gt.border = new RectOffset(0, 0, 0, 0);
            gt.enabled = true;
            gt.transform.position = Vector3.zero;
            if (GetComponent<Camera>().velocity.sqrMagnitude < 322*322)
            {
                gt.color = Color.Lerp(Color.clear, Color.blue, GetComponent<Camera>().velocity.sqrMagnitude/(322 * 322));
            }
            else if (GetComponent<Camera>().velocity.sqrMagnitude < 322 * 322 * 9)
            {
                gt.color = Color.Lerp(Color.blue, Color.red, GetComponent<Camera>().velocity.sqrMagnitude / (322 * 322 * 9));
            }
        }
    }
}
