#if UNITY_CHANGE1 || UNITY_CHANGE2 || UNITY_CHANGE3
#warning UNITY_CHANGE has been set manually
#elif UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
#define UNITY_CHANGE1
#elif UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#define UNITY_CHANGE2
#else
#define UNITY_CHANGE3
#endif
//use UNITY_CHANGE1 for unity older than "unity 5"
//use UNITY_CHANGE2 for unity 5.0 -> 5.3 
//use UNITY_CHANGE3 for unity 5.3 (fix for new SceneManger system  )


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
#if UNITY_CHANGE3
using UnityEngine.SceneManagement;

#endif


[Serializable]
public class Images
{
  public Texture2D clearImage;
  public Texture2D collapseImage;
  public Texture2D clearOnNewSceneImage;
  public Texture2D showTimeImage;
  public Texture2D showSceneImage;
  public Texture2D userImage;
  public Texture2D showMemoryImage;
  public Texture2D softwareImage;
  public Texture2D dateImage;
  public Texture2D showFpsImage;
  public Texture2D infoImage;
  public Texture2D searchImage;
  public Texture2D closeImage;

  public Texture2D buildFromImage;
  public Texture2D systemInfoImage;
  public Texture2D graphicsInfoImage;
  public Texture2D backImage;

  public Texture2D logImage;
  public Texture2D warningImage;
  public Texture2D errorImage;

  public Texture2D barImage;
  public Texture2D button_activeImage;
  public Texture2D even_logImage;
  public Texture2D odd_logImage;
  public Texture2D selectedImage;

  public GUISkin reporterScrollerSkin;
}

//To use Reporter just create reporter from menu (Reporter->Create) at first scene your game start.
//then set the ” Scrip execution order ” in (Edit -> Project Settings ) of Reporter.cs to be the highest.

//Now to PanelView logs all what you have to do is to make a circle gesture using your mouse (click and drag)
//or your finger (touch and drag) on the screen to show all these logs
//no coding is required

public class Reporter : MonoBehaviour
{
  public enum _LogType
  {
    Assert = LogType.Assert,
    Error = LogType.Error,
    Exception = LogType.Exception,
    Log = LogType.Log,
    Warning = LogType.Warning,
  }

  public class Sample
  {
    public float time;
    public int loadedScene;
    public float memory;
    public float fps;
    public string fpsText;

    public static float MemSize()
    {
      float s = sizeof(float) + sizeof(byte) + sizeof(float) + sizeof(float);
      return s;
    }

    public string GetSceneName()
    {
      return loadedScene == -1 ? "AssetBundleScene" : scenes[loadedScene];
    }
  }

  private readonly List<Sample> samples = new List<Sample>(60 * 60 * 60);

  public class Log
  {
    public int count = 1;
    public _LogType logType;
    public string condition;
    public string stacktrace;

    public int sampleId;
    //public string   objectName="" ;//object who send error
    //public string   rootName =""; //root of object send error

    public float GetMemoryUsage()
    {
      return (sizeof(int) +
              sizeof(_LogType) +
              condition.Length * sizeof(char) +
              stacktrace.Length * sizeof(char) +
              sizeof(int));
    }
  }

  //contains all uncollapsed log
  private readonly List<Log> logs = new List<Log>();

  //contains all collapsed logs
  private readonly List<Log> collapsedLogs = new List<Log>();

  //contain logs which should only appear to user , for example if you switch off show logs + switch off show warnings
  //and your mode is collapse,then this list will contains only collapsed errors
  private readonly List<Log> currentLog = new List<Log>();

  //used to check if the new coming logs is already exist or new one
  private readonly MultiKeyDictionary<string, string, Log> logsDic = new MultiKeyDictionary<string, string, Log>();

  //to save memory
  private readonly Dictionary<string, string> cachedString = new Dictionary<string, string>();

  [HideInInspector]
  //show hide In Game Logs
  public bool show;

  //collapse logs
  private bool collapse;

  //to decide if you want to clean logs for new loaded scene
  private bool clearOnNewSceneLoaded;

  private bool showTime;

  private bool showScene;

  private bool showMemory;

  private bool showFps;

  private bool showGraph;

  //show or hide logs
  private bool showLog = true;

  //show or hide warnings
  private bool showWarning = true;

  //show or hide errors
  private bool showError = true;

  //total number of logs
  private int numOfLogs;

  //total number of warnings
  private int numOfLogsWarning;

  //total number of errors
  private int numOfLogsError;

  //total number of collapsed logs
  private int numOfCollapsedLogs;

  //total number of collapsed warnings
  private int numOfCollapsedLogsWarning;

  //total number of collapsed errors
  private int numOfCollapsedLogsError;

  //maximum number of allowed logs to PanelView
  //public int maxAllowedLog = 1000 ;

  private bool showClearOnNewSceneLoadedButton = true;
  private bool showTimeButton = true;
  private bool showSceneButton = true;
  private bool showMemButton = true;
  private bool showFpsButton = true;
  private bool showSearchText = true;

  private string buildDate;
  private string logDate;
  private float logsMemUsage;
  private float graphMemUsage;

  public float TotalMemUsage
  {
    get { return logsMemUsage + graphMemUsage; }
  }

  private float gcTotalMemory;

  public string UserData = "";

  //frame rate per second
  public float fps;
  public string fpsText;

  //ListTroopers<Texture2D> snapshots = new ListTroopers<Texture2D>() ;

  enum ReportView
  {
    Logs,
    Info,
  }

  private ReportView currentView = ReportView.Logs;

  //used to check if you have In Game Logs multiple time in different scene
  //only one should work and other should be deleted
  static bool created;
  //public delegate void OnLogHandler( string condition, string stack-trace, LogType type );
  //public event OnLogHandler OnLog ;

  public Images images;

  // gui
  GUIContent clearContent;
  GUIContent collapseContent;
  GUIContent clearOnNewSceneContent;
  GUIContent showTimeContent;
  GUIContent showSceneContent;
  GUIContent userContent;
  GUIContent showMemoryContent;
  GUIContent softwareContent;
  GUIContent dateContent;

  GUIContent showFpsContent;

  //GUIContent graphContent;
  GUIContent infoContent;
  GUIContent searchContent;
  GUIContent closeContent;

  GUIContent buildFromContent;
  GUIContent systemInfoContent;
  GUIContent graphicsInfoContent;
  GUIContent backContent;

  //GUIContent cameraContent;

  GUIContent logContent;
  GUIContent warningContent;
  GUIContent errorContent;
  GUIStyle barStyle;
  GUIStyle buttonActiveStyle;

  GUIStyle nonStyle;
  GUIStyle lowerLeftFontStyle;
  GUIStyle backStyle;
  GUIStyle evenLogStyle;
  GUIStyle oddLogStyle;
  GUIStyle logButtonStyle;
  GUIStyle selectedLogStyle;
  GUIStyle selectedLogFontStyle;
  GUIStyle stackLabelStyle;
  GUIStyle searchStyle;
  GUIStyle sliderBackStyle;
  GUIStyle sliderThumbStyle;
  GUISkin toolbarScrollerSkin;
  GUISkin logScrollerSkin;
  GUISkin graphScrollerSkin;

  public Vector2 size = new Vector2(32, 32);
  public float maxSize = 20;
  public int numOfCircleToShow = 1;
  static string[] scenes;
  string currentScene;
  string filterText = "";

  string deviceModel;
  string deviceType;
  string deviceName;
  string graphicsMemorySize;
#if !UNITY_CHANGE1
  string maxTextureSize;
#endif
  string systemMemorySize;

  private void Awake()
  {
#if !DEVELOPMENT_BUILD
Destroy(gameObject);
    #endif
    if (!Initialized)
      Initialize();
  }

  private void OnEnable()
  {
    if (logs.Count == 0) //if recompile while in play mode
      clear();
  }

  private void addSample()
  {
    Sample sample = new Sample
    {
      fps = fps,
      fpsText = fpsText,
      loadedScene = SceneManager.GetActiveScene().buildIndex,
      time = Time.realtimeSinceStartup,
      memory = gcTotalMemory
    };
#if UNITY_CHANGE3
#else
		sample.loadedScene = (byte)Application.loadedLevel;
#endif
    samples.Add(sample);

    graphMemUsage = (samples.Count * Sample.MemSize()) / 1024 / 1024;
  }

  public bool Initialized;

  public void Initialize()
  {
    if (!created)
    {
      try
      {
        gameObject.SendMessage("OnPreStart");
      }
      catch (Exception e)
      {
        Debug.LogException(e);
      }
#if UNITY_CHANGE3
      scenes = new string[SceneManager.sceneCountInBuildSettings];
      currentScene = SceneManager.GetActiveScene().name;
#else
			scenes = new string[Application.levelCount];
			currentScene = Application.loadedLevelName;
#endif
      DontDestroyOnLoad(gameObject);
#if UNITY_CHANGE1
			Application.RegisterLogCallback (new Application.LogCallback (CaptureLog));
			Application.RegisterLogCallbackThreaded (new Application.LogCallback (CaptureLogThread));
#else
      //Application.logMessageReceived += CaptureLog ;
      Application.logMessageReceivedThreaded += CaptureLogThread;
#endif
      created = true;
      //addSample();
    }
    else
    {
      Debug.LogWarning("tow manager is exists delete the second");
      DestroyImmediate(gameObject, true);
      return;
    }


    //initialize gui and styles for gui purpose

    clearContent = new GUIContent("", images.clearImage, "Clear logs");
    collapseContent = new GUIContent("", images.collapseImage, "Collapse logs");
    clearOnNewSceneContent = new GUIContent("", images.clearOnNewSceneImage, "Clear logs on new scene loaded");
    showTimeContent = new GUIContent("", images.showTimeImage, "Show Hide Time");
    showSceneContent = new GUIContent("", images.showSceneImage, "Show Hide Scene");
    showMemoryContent = new GUIContent("", images.showMemoryImage, "Show Hide Memory");
    softwareContent = new GUIContent("", images.softwareImage, "Software");
    dateContent = new GUIContent("", images.dateImage, "Date");
    showFpsContent = new GUIContent("", images.showFpsImage, "Show Hide fps");
    infoContent = new GUIContent("", images.infoImage, "Information about application");
    searchContent = new GUIContent("", images.searchImage, "Search for logs");
    closeContent = new GUIContent("", images.closeImage, "Hide logs");
    userContent = new GUIContent("", images.userImage, "User");

    buildFromContent = new GUIContent("", images.buildFromImage, "Build From");
    systemInfoContent = new GUIContent("", images.systemInfoImage, "System Info");
    graphicsInfoContent = new GUIContent("", images.graphicsInfoImage, "Graphics Info");
    backContent = new GUIContent("", images.backImage, "Back");


    //snapshotContent = new GUIContent("",images.cameraImage,"show or hide logs");
    logContent = new GUIContent("", images.logImage, "show or hide logs");
    warningContent = new GUIContent("", images.warningImage, "show or hide warnings");
    errorContent = new GUIContent("", images.errorImage, "show or hide errors");


    currentView = (ReportView) PlayerPrefs.GetInt("Reporter_currentView", 1);
    show = (PlayerPrefs.GetInt("Reporter_show") == 1);
    collapse = (PlayerPrefs.GetInt("Reporter_collapse") == 1);
    clearOnNewSceneLoaded = (PlayerPrefs.GetInt("Reporter_clearOnNewSceneLoaded") == 1);
    showTime = (PlayerPrefs.GetInt("Reporter_showTime") == 1);
    showScene = (PlayerPrefs.GetInt("Reporter_showScene") == 1);
    showMemory = (PlayerPrefs.GetInt("Reporter_showMemory") == 1);
    showFps = (PlayerPrefs.GetInt("Reporter_showFps") == 1);
    showGraph = (PlayerPrefs.GetInt("Reporter_showGraph") == 1);
    showLog = (PlayerPrefs.GetInt("Reporter_showLog", 1) == 1);
    showWarning = (PlayerPrefs.GetInt("Reporter_showWarning", 1) == 1);
    showError = (PlayerPrefs.GetInt("Reporter_showError", 1) == 1);
    filterText = PlayerPrefs.GetString("Reporter_filterText");
    size.x = size.y = PlayerPrefs.GetFloat("Reporter_size", 32);


    showClearOnNewSceneLoadedButton =
      (PlayerPrefs.GetInt("Reporter_showClearOnNewSceneLoadedButton", 1) == 1);
    showTimeButton = (PlayerPrefs.GetInt("Reporter_showTimeButton", 1) == 1);
    showSceneButton = (PlayerPrefs.GetInt("Reporter_showSceneButton", 1) == 1);
    showMemButton = (PlayerPrefs.GetInt("Reporter_showMemButton", 1) == 1);
    showFpsButton = (PlayerPrefs.GetInt("Reporter_showFpsButton", 1) == 1);
    showSearchText = (PlayerPrefs.GetInt("Reporter_showSearchText", 1) == 1);


    initializeStyle();

    Initialized = true;

    if (show)
    {
      doShow();
    }

    deviceModel = SystemInfo.deviceModel;
    deviceType = SystemInfo.deviceType.ToString();
    deviceName = SystemInfo.deviceName;
    graphicsMemorySize = SystemInfo.graphicsMemorySize.ToString();
#if !UNITY_CHANGE1
    maxTextureSize = SystemInfo.maxTextureSize.ToString();
#endif
    systemMemorySize = SystemInfo.systemMemorySize.ToString();
  }

  void initializeStyle()
  {
    int paddingX = (int) (size.x * 0.2f);
    int paddingY = (int) (size.y * 0.2f);
    nonStyle = new GUIStyle
    {
      clipping = TextClipping.Clip,
      border = new RectOffset(0, 0, 0, 0),
      normal = {background = null},
      fontSize = (int) (size.y / 2),
      alignment = TextAnchor.MiddleCenter
    };

    lowerLeftFontStyle = new GUIStyle
    {
      clipping = TextClipping.Clip,
      border = new RectOffset(0, 0, 0, 0),
      normal = {background = null},
      fontSize = (int) (size.y / 2),
      fontStyle = FontStyle.Bold,
      alignment = TextAnchor.LowerLeft
    };


    barStyle = new GUIStyle
    {
      border = new RectOffset(1, 1, 1, 1),
      normal = {background = images.barImage},
      active = {background = images.button_activeImage},
      alignment = TextAnchor.MiddleCenter,
      margin = new RectOffset(1, 1, 1, 1),
      clipping = TextClipping.Clip,
      fontSize = (int) (size.y / 2)
    };

    //barStyle.padding = new RectOffset(paddingX,paddingX,paddingY,paddingY);
    //barStyle.wordWrap = true ;


    buttonActiveStyle = new GUIStyle
    {
      border = new RectOffset(1, 1, 1, 1),
      normal = {background = images.button_activeImage},
      alignment = TextAnchor.MiddleCenter,
      margin = new RectOffset(1, 1, 1, 1),
      fontSize = (int) (size.y / 2)
    };
    //buttonActiveStyle.padding = new RectOffset(4,4,4,4);

    backStyle = new GUIStyle
    {
      normal = {background = images.even_logImage},
      clipping = TextClipping.Clip,
      fontSize = (int) (size.y / 2)
    };

    evenLogStyle = new GUIStyle
    {
      normal = {background = images.even_logImage},
      fixedHeight = size.y,
      clipping = TextClipping.Clip,
      alignment = TextAnchor.UpperLeft,
      imagePosition = ImagePosition.ImageLeft,
      fontSize = (int) (size.y / 2)
    };
    //evenLogStyle.wordWrap = true;

    oddLogStyle = new GUIStyle
    {
      normal = {background = images.odd_logImage},
      fixedHeight = size.y,
      clipping = TextClipping.Clip,
      alignment = TextAnchor.UpperLeft,
      imagePosition = ImagePosition.ImageLeft,
      fontSize = (int) (size.y / 2)
    };
    //oddLogStyle.wordWrap = true ;

    logButtonStyle = new GUIStyle
    {
      fixedHeight = size.y,
      clipping = TextClipping.Clip,
      alignment = TextAnchor.UpperLeft,
      fontSize = (int) (size.y / 2),
      padding = new RectOffset(paddingX, paddingX, paddingY, paddingY)
    };
    //logButtonStyle.wordWrap = true;
    //logButtonStyle.imagePosition = ImagePosition.ImageLeft ;
    //logButtonStyle.wordWrap = true;

    selectedLogStyle = new GUIStyle
    {
      normal = {background = images.selectedImage},
      fixedHeight = size.y,
      clipping = TextClipping.Clip,
      alignment = TextAnchor.UpperLeft
    };
    selectedLogStyle.normal.textColor = Color.white;
    //selectedLogStyle.wordWrap = true;
    selectedLogStyle.fontSize = (int) (size.y / 2);

    selectedLogFontStyle = new GUIStyle
    {
      normal = {background = images.selectedImage},
      fixedHeight = size.y,
      clipping = TextClipping.Clip,
      alignment = TextAnchor.UpperLeft
    };
    selectedLogFontStyle.normal.textColor = Color.white;
    //selectedLogStyle.wordWrap = true;
    selectedLogFontStyle.fontSize = (int) (size.y / 2);
    selectedLogFontStyle.padding = new RectOffset(paddingX, paddingX, paddingY, paddingY);

    stackLabelStyle = new GUIStyle
    {
      wordWrap = true,
      fontSize = (int) (size.y / 2),
      padding = new RectOffset(paddingX, paddingX, paddingY, paddingY)
    };

    searchStyle = new GUIStyle
    {
      clipping = TextClipping.Clip,
      alignment = TextAnchor.LowerCenter,
      fontSize = (int) (size.y / 2),
      wordWrap = true
    };


    sliderBackStyle = new GUIStyle
    {
      normal = {background = images.barImage},
      fixedHeight = size.y,
      border = new RectOffset(1, 1, 1, 1)
    };

    sliderThumbStyle = new GUIStyle
    {
      normal = {background = images.selectedImage},
      fixedWidth = size.x
    };

    GUISkin skin = images.reporterScrollerSkin;

    toolbarScrollerSkin = Instantiate(skin);
    toolbarScrollerSkin.verticalScrollbar.fixedWidth = 0f;
    toolbarScrollerSkin.horizontalScrollbar.fixedHeight = 0f;
    toolbarScrollerSkin.verticalScrollbarThumb.fixedWidth = 0f;
    toolbarScrollerSkin.horizontalScrollbarThumb.fixedHeight = 0f;

    logScrollerSkin = Instantiate(skin);
    logScrollerSkin.verticalScrollbar.fixedWidth = size.x * 2f;
    logScrollerSkin.horizontalScrollbar.fixedHeight = 0f;
    logScrollerSkin.verticalScrollbarThumb.fixedWidth = size.x * 2f;
    logScrollerSkin.horizontalScrollbarThumb.fixedHeight = 0f;

    graphScrollerSkin = Instantiate(skin);
    graphScrollerSkin.verticalScrollbar.fixedWidth = 0f;
    graphScrollerSkin.horizontalScrollbar.fixedHeight = size.x * 2f;
    graphScrollerSkin.verticalScrollbarThumb.fixedWidth = 0f;
    graphScrollerSkin.horizontalScrollbarThumb.fixedHeight = size.x * 2f;
    //inGameLogsScrollerSkin.verticalScrollbarThumb.fixedWidth = size.x * 2;
    //inGameLogsScrollerSkin.verticalScrollbar.fixedWidth = size.x * 2;
  }

  private void Start()
  {
    logDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
    StartCoroutine("readInfo");
  }

  //clear all logs
  void clear()
  {
    logs.Clear();
    collapsedLogs.Clear();
    currentLog.Clear();
    logsDic.Clear();
    //selectedIndex = -1;
    selectedLog = null;
    numOfLogs = 0;
    numOfLogsWarning = 0;
    numOfLogsError = 0;
    numOfCollapsedLogs = 0;
    numOfCollapsedLogsWarning = 0;
    numOfCollapsedLogsError = 0;
    logsMemUsage = 0;
    graphMemUsage = 0;
    samples.Clear();
    GC.Collect();
    selectedLog = null;
  }

  public Rect ScreenRect;
  public Rect ToolBarRect;
  private Rect logsRect;
  private Rect stackRect;
  private Rect graphRect;
  private Rect graphMinRect;
  private Rect graphMaxRect;
  public Rect ButtomRect;
  public Rect DetailRect;

  private Vector2 scrollPosition;
  private Vector2 scrollPosition2;
  private Vector2 toolbarScrollPosition;

  //int 	selectedIndex = -1;
  private Log selectedLog;

  private float toolbarOldDrag;
  private float oldDrag;
  private float oldDrag2;
  private float oldDrag3;
  private int startIndex;

  //calculate what is the currentLog : collapsed or not , hide or PanelView warnings ......
  private void calculateCurrentLog()
  {
    bool filter = !string.IsNullOrEmpty(filterText);
    string _filterText = "";
    if (filter)
      _filterText = filterText.ToLower();
    currentLog.Clear();
    if (collapse)
    {
      foreach (var log in collapsedLogs)
      {
        if (log.logType == _LogType.Log && !showLog)
          continue;
        if (log.logType == _LogType.Warning && !showWarning)
          continue;
        if (log.logType == _LogType.Error && !showError)
          continue;
        if (log.logType == _LogType.Assert && !showError)
          continue;
        if (log.logType == _LogType.Exception && !showError)
          continue;

        if (filter)
        {
          if (log.condition.ToLower().Contains(_filterText))
            currentLog.Add(log);
        }
        else
        {
          currentLog.Add(log);
        }
      }
    }
    else
    {
      foreach (var log in logs)
      {
        if (log.logType == _LogType.Log && !showLog)
          continue;
        if (log.logType == _LogType.Warning && !showWarning)
          continue;
        if (log.logType == _LogType.Error && !showError)
          continue;
        if (log.logType == _LogType.Assert && !showError)
          continue;
        if (log.logType == _LogType.Exception && !showError)
          continue;

        if (filter)
        {
          if (log.condition.ToLower().Contains(_filterText))
            currentLog.Add(log);
        }
        else
        {
          currentLog.Add(log);
        }
      }
    }

    if (selectedLog != null)
    {
      int newSelectedIndex = currentLog.IndexOf(selectedLog);
      if (newSelectedIndex == -1)
      {
        Log collapsedSelected = logsDic[selectedLog.condition][selectedLog.stacktrace];
        newSelectedIndex = currentLog.IndexOf(collapsedSelected);
        if (newSelectedIndex != -1)
          scrollPosition.y = newSelectedIndex * size.y;
      }
      else
      {
        scrollPosition.y = newSelectedIndex * size.y;
      }
    }
  }

  public Rect CountRect;
  private Rect timeRect;
  private Rect timeLabelRect;
  private Rect sceneRect;
  private Rect sceneLabelRect;
  private Rect memoryRect;
  private Rect memoryLabelRect;
  private Rect fpsRect;
  private Rect fpsLabelRect;
  private readonly GUIContent tempContent = new GUIContent();


  private Vector2 infoScrollPosition;
  private Vector2 oldInfoDrag;

  private void DrawInfo()
  {
    GUILayout.BeginArea(ScreenRect, backStyle);

    Vector2 drag = getDrag();
    if ((Math.Abs(drag.x) > .01) && (downPos != Vector2.zero))
    {
      infoScrollPosition.x -= (drag.x - oldInfoDrag.x);
    }

    if ((Math.Abs(drag.y) > .01) && (downPos != Vector2.zero))
    {
      infoScrollPosition.y += (drag.y - oldInfoDrag.y);
    }

    oldInfoDrag = drag;

    GUI.skin = toolbarScrollerSkin;
    infoScrollPosition = GUILayout.BeginScrollView(infoScrollPosition);
    GUILayout.Space(size.x);
    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Box(buildFromContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(buildDate, nonStyle, GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Box(systemInfoContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(deviceModel, nonStyle, GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(deviceType, nonStyle, GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(deviceName, nonStyle, GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Box(graphicsInfoContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(SystemInfo.graphicsDeviceName, nonStyle, GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(graphicsMemorySize, nonStyle, GUILayout.Height(size.y));
#if !UNITY_CHANGE1
    GUILayout.Space(size.x);
    GUILayout.Label(maxTextureSize, nonStyle, GUILayout.Height(size.y));
#endif
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Space(size.x);
    GUILayout.Space(size.x);
    GUILayout.Label("Screen Width " + Screen.width, nonStyle, GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label("Screen Height " + Screen.height, nonStyle, GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Box(showMemoryContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(systemMemorySize + " mb", nonStyle, GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Space(size.x);
    GUILayout.Space(size.x);
    GUILayout.Label("Mem Usage Of Logs " + logsMemUsage.ToString("0.000") + " mb", nonStyle, GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    //GUILayout.Label( "Mem Usage Of Graph " + graphMemUsage.ToString("0.000")  + " mb", nonStyle , GUILayout.Height(size.y));
    //GUILayout.Space( size.x);
    GUILayout.Label("GC Memory " + gcTotalMemory.ToString("0.000") + " mb", nonStyle, GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Box(softwareContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(SystemInfo.operatingSystem, nonStyle, GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();


    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Box(dateContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(DateTime.Now.ToString(CultureInfo.InvariantCulture), nonStyle, GUILayout.Height(size.y));
    GUILayout.Label(" - Application Started At " + logDate, nonStyle, GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Box(showTimeContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(Time.realtimeSinceStartup.ToString("000"), nonStyle, GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Box(showFpsContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(fpsText, nonStyle, GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Box(userContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(UserData, nonStyle, GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Box(showSceneContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label(currentScene, nonStyle, GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Box(showSceneContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.Label("Unity Version = " + Application.unityVersion, nonStyle, GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    /*GUILayout.BeginHorizontal();
    GUILayout.Space( size.x);
    GUILayout.Box( graphContent ,nonStyle ,  GUILayout.Width(size.x) , GUILayout.Height(size.y));
    GUILayout.Space( size.x);
    GUILayout.Label( "frame " + samples.Count , nonStyle , GUILayout.Height(size.y));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();*/

    drawInfo_enableDisableToolBarButtons();

    GUILayout.FlexibleSpace();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Label("Size = " + size.x.ToString("0.0"), nonStyle, GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    float _size = GUILayout.HorizontalSlider(size.x, 16, 64, sliderBackStyle, sliderThumbStyle,
      GUILayout.Width(Screen.width * 0.5f));
    if (Math.Abs(size.x - _size) > .01)
    {
      size.x = size.y = _size;
      initializeStyle();
    }

    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    if (GUILayout.Button(backContent, barStyle, GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      currentView = ReportView.Logs;
    }

    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();


    GUILayout.EndScrollView();

    GUILayout.EndArea();
  }


  void drawInfo_enableDisableToolBarButtons()
  {
    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);
    GUILayout.Label("Hide or Show tool bar buttons", nonStyle, GUILayout.Height(size.y));
    GUILayout.Space(size.x);
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Space(size.x);

    if (GUILayout.Button(clearOnNewSceneContent, (showClearOnNewSceneLoadedButton) ? buttonActiveStyle : barStyle,
      GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      showClearOnNewSceneLoadedButton = !showClearOnNewSceneLoadedButton;
    }

    if (GUILayout.Button(showTimeContent, (showTimeButton) ? buttonActiveStyle : barStyle, GUILayout.Width(size.x * 2),
      GUILayout.Height(size.y * 2)))
    {
      showTimeButton = !showTimeButton;
    }

    tempRect = GUILayoutUtility.GetLastRect();
    GUI.Label(tempRect, Time.realtimeSinceStartup.ToString("0.0"), lowerLeftFontStyle);
    if (GUILayout.Button(showSceneContent, (showSceneButton) ? buttonActiveStyle : barStyle,
      GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      showSceneButton = !showSceneButton;
    }

    tempRect = GUILayoutUtility.GetLastRect();
    GUI.Label(tempRect, currentScene, lowerLeftFontStyle);
    if (GUILayout.Button(showMemoryContent, (showMemButton) ? buttonActiveStyle : barStyle, GUILayout.Width(size.x * 2),
      GUILayout.Height(size.y * 2)))
    {
      showMemButton = !showMemButton;
    }

    tempRect = GUILayoutUtility.GetLastRect();
    GUI.Label(tempRect, gcTotalMemory.ToString("0.0"), lowerLeftFontStyle);

    if (GUILayout.Button(showFpsContent, (showFpsButton) ? buttonActiveStyle : barStyle, GUILayout.Width(size.x * 2),
      GUILayout.Height(size.y * 2)))
    {
      showFpsButton = !showFpsButton;
    }

    tempRect = GUILayoutUtility.GetLastRect();
    GUI.Label(tempRect, fpsText, lowerLeftFontStyle);
    /*if( GUILayout.Button( graphContent , (showGraph)?buttonActiveStyle:barStyle , GUILayout.Width(size.x*2) ,GUILayout.Height(size.y*2)))
    {
      showGraph = !showGraph ;
    }
    tempRect = GUILayoutUtility.GetLastRect();
    GUI.Label( tempRect , samples.Count.ToString() , lowerLeftFontStyle );*/
    if (GUILayout.Button(searchContent, (showSearchText) ? buttonActiveStyle : barStyle, GUILayout.Width(size.x * 2),
      GUILayout.Height(size.y * 2)))
    {
      showSearchText = !showSearchText;
    }

    tempRect = GUILayoutUtility.GetLastRect();
    GUI.TextField(tempRect, filterText, searchStyle);


    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();
  }

  void drawToolBar()
  {
    ToolBarRect.x = 0f;
    ToolBarRect.y = 0f;
    ToolBarRect.width = Screen.width;
    ToolBarRect.height = size.y * 2f;

    //toolbarScrollerSkin.verticalScrollbar.fixedWidth = 0f;
    //toolbarScrollerSkin.horizontalScrollbar.fixedHeight= 0f  ;

    GUI.skin = toolbarScrollerSkin;
    Vector2 drag = getDrag();
    if ((Math.Abs(drag.x) > .01) && (downPos != Vector2.zero) && (downPos.y > Screen.height - size.y * 2f))
    {
      toolbarScrollPosition.x -= (drag.x - toolbarOldDrag);
    }

    toolbarOldDrag = drag.x;
    GUILayout.BeginArea(ToolBarRect);
    toolbarScrollPosition = GUILayout.BeginScrollView(toolbarScrollPosition);
    GUILayout.BeginHorizontal(barStyle);

    if (GUILayout.Button(clearContent, barStyle, GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      clear();
    }

    if (GUILayout.Button(collapseContent, (collapse) ? buttonActiveStyle : barStyle, GUILayout.Width(size.x * 2),
      GUILayout.Height(size.y * 2)))
    {
      collapse = !collapse;
      calculateCurrentLog();
    }

    if (showClearOnNewSceneLoadedButton && GUILayout.Button(clearOnNewSceneContent,
          (clearOnNewSceneLoaded) ? buttonActiveStyle : barStyle, GUILayout.Width(size.x * 2),
          GUILayout.Height(size.y * 2)))
    {
      clearOnNewSceneLoaded = !clearOnNewSceneLoaded;
    }

    if (showTimeButton && GUILayout.Button(showTimeContent, (showTime) ? buttonActiveStyle : barStyle,
          GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      showTime = !showTime;
    }

    if (showSceneButton)
    {
      tempRect = GUILayoutUtility.GetLastRect();
      GUI.Label(tempRect, Time.realtimeSinceStartup.ToString("0.0"), lowerLeftFontStyle);
      if (GUILayout.Button(showSceneContent, (showScene) ? buttonActiveStyle : barStyle, GUILayout.Width(size.x * 2),
        GUILayout.Height(size.y * 2)))
      {
        showScene = !showScene;
      }

      tempRect = GUILayoutUtility.GetLastRect();
      GUI.Label(tempRect, currentScene, lowerLeftFontStyle);
    }

    if (showMemButton)
    {
      if (GUILayout.Button(showMemoryContent, (showMemory) ? buttonActiveStyle : barStyle, GUILayout.Width(size.x * 2),
        GUILayout.Height(size.y * 2)))
      {
        showMemory = !showMemory;
      }

      tempRect = GUILayoutUtility.GetLastRect();
      GUI.Label(tempRect, gcTotalMemory.ToString("0.0"), lowerLeftFontStyle);
    }

    if (showFpsButton)
    {
      if (GUILayout.Button(showFpsContent, (showFps) ? buttonActiveStyle : barStyle, GUILayout.Width(size.x * 2),
        GUILayout.Height(size.y * 2)))
      {
        showFps = !showFps;
      }

      tempRect = GUILayoutUtility.GetLastRect();
      GUI.Label(tempRect, fpsText, lowerLeftFontStyle);
    }
    /*if( GUILayout.Button( graphContent , (showGraph)?buttonActiveStyle:barStyle , GUILayout.Width(size.x*2) ,GUILayout.Height(size.y*2)))
    {
      showGraph = !showGraph ;
    }
    tempRect = GUILayoutUtility.GetLastRect();
    GUI.Label( tempRect , samples.Count.ToString() , lowerLeftFontStyle );*/

    if (showSearchText)
    {
      GUILayout.Box(searchContent, barStyle, GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2));
      tempRect = GUILayoutUtility.GetLastRect();
      string newFilterText = GUI.TextField(tempRect, filterText, searchStyle);
      if (newFilterText != filterText)
      {
        filterText = newFilterText;
        calculateCurrentLog();
      }
    }

    if (GUILayout.Button(infoContent, barStyle, GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      currentView = ReportView.Info;
    }


    GUILayout.FlexibleSpace();


    string logsText = " ";
    if (collapse)
    {
      logsText += numOfCollapsedLogs;
    }
    else
    {
      logsText += numOfLogs;
    }

    string logsWarningText = " ";
    if (collapse)
    {
      logsWarningText += numOfCollapsedLogsWarning;
    }
    else
    {
      logsWarningText += numOfLogsWarning;
    }

    string logsErrorText = " ";
    if (collapse)
    {
      logsErrorText += numOfCollapsedLogsError;
    }
    else
    {
      logsErrorText += numOfLogsError;
    }

    GUILayout.BeginHorizontal((showLog) ? buttonActiveStyle : barStyle);
    if (GUILayout.Button(logContent, nonStyle, GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      showLog = !showLog;
      calculateCurrentLog();
    }

    if (GUILayout.Button(logsText, nonStyle, GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      showLog = !showLog;
      calculateCurrentLog();
    }

    GUILayout.EndHorizontal();
    GUILayout.BeginHorizontal((showWarning) ? buttonActiveStyle : barStyle);
    if (GUILayout.Button(warningContent, nonStyle, GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      showWarning = !showWarning;
      calculateCurrentLog();
    }

    if (GUILayout.Button(logsWarningText, nonStyle, GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      showWarning = !showWarning;
      calculateCurrentLog();
    }

    GUILayout.EndHorizontal();
    GUILayout.BeginHorizontal((showError) ? buttonActiveStyle : nonStyle);
    if (GUILayout.Button(errorContent, nonStyle, GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      showError = !showError;
      calculateCurrentLog();
    }

    if (GUILayout.Button(logsErrorText, nonStyle, GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      showError = !showError;
      calculateCurrentLog();
    }

    GUILayout.EndHorizontal();

    if (GUILayout.Button(closeContent, barStyle, GUILayout.Width(size.x * 2), GUILayout.Height(size.y * 2)))
    {
      show = false;
      ReporterGUI gui = gameObject.GetComponent<ReporterGUI>();
      DestroyImmediate(gui);

      try
      {
        gameObject.SendMessage("OnHideReporter");
      }
      catch (Exception e)
      {
        Debug.LogException(e);
      }
    }


    GUILayout.EndHorizontal();

    GUILayout.EndScrollView();

    GUILayout.EndArea();
  }


  private Rect tempRect;

  void DrawLogs()
  {
    GUILayout.BeginArea(logsRect, backStyle);

    GUI.skin = logScrollerSkin;
    //setStartPos();
    Vector2 drag = getDrag();

    if (Math.Abs(drag.y) > .01 && logsRect.Contains(new Vector2(downPos.x, Screen.height - downPos.y)))
    {
      scrollPosition.y += (drag.y - oldDrag);
    }

    scrollPosition = GUILayout.BeginScrollView(scrollPosition);

    oldDrag = drag.y;


    int totalVisibleCount = (int) (Screen.height * 0.75f / size.y);
    int totalCount = currentLog.Count;
    /*if( totalCount < 100 )
      inGameLogsScrollerSkin.verticalScrollbarThumb.fixedHeight = 0;
    else
      inGameLogsScrollerSkin.verticalScrollbarThumb.fixedHeight = 64;*/

    totalVisibleCount = Mathf.Min(totalVisibleCount, totalCount - startIndex);
    int index = 0;
    int beforeHeight = (int) (startIndex * size.y);
    //selectedIndex = Mathf.Clamp( selectedIndex , -1 , totalCount -1);
    if (beforeHeight > 0)
    {
      //fill invisible gap before scroller to make proper scroller pos
      GUILayout.BeginHorizontal(GUILayout.Height(beforeHeight));
      GUILayout.Label("---");
      GUILayout.EndHorizontal();
    }

    int endIndex = startIndex + totalVisibleCount;
    endIndex = Mathf.Clamp(endIndex, 0, totalCount);
    bool scrollerVisible = (totalVisibleCount < totalCount);
    for (int i = startIndex; (startIndex + index) < endIndex; i++)
    {
      if (i >= currentLog.Count)
        break;
      Log log = currentLog[i];

      if (log.logType == _LogType.Log && !showLog)
        continue;
      if (log.logType == _LogType.Warning && !showWarning)
        continue;
      if (log.logType == _LogType.Error && !showError)
        continue;
      if (log.logType == _LogType.Assert && !showError)
        continue;
      if (log.logType == _LogType.Exception && !showError)
        continue;

      if (index >= totalVisibleCount)
      {
        break;
      }

      GUIContent content;
      if (log.logType == _LogType.Log)
        content = logContent;
      else if (log.logType == _LogType.Warning)
        content = warningContent;
      else
        content = errorContent;
      //content.text = log.condition ;

      GUIStyle currentLogStyle = ((startIndex + index) % 2 == 0) ? evenLogStyle : oddLogStyle;
      if (log == selectedLog)
      {
        //selectedLog = log ;
        currentLogStyle = selectedLogStyle;
      }
      else
      {
      }

      tempContent.text = log.count.ToString();
      float w = 0f;
      if (collapse)
        w = barStyle.CalcSize(tempContent).x + 3;
      CountRect.x = Screen.width - w;
      CountRect.y = size.y * i;
      if (beforeHeight > 0)
        CountRect.y += 8; //i will check later why
      CountRect.width = w;
      CountRect.height = size.y;

      if (scrollerVisible)
        CountRect.x -= size.x * 2;

      Sample sample = samples[log.sampleId];
      fpsRect = CountRect;
      if (showFps)
      {
        tempContent.text = sample.fpsText;
        w = currentLogStyle.CalcSize(tempContent).x + size.x;
        fpsRect.x -= w;
        fpsRect.width = size.x;
        fpsLabelRect = fpsRect;
        fpsLabelRect.x += size.x;
        fpsLabelRect.width = w - size.x;
      }


      memoryRect = fpsRect;
      if (showMemory)
      {
        tempContent.text = sample.memory.ToString("0.000");
        w = currentLogStyle.CalcSize(tempContent).x + size.x;
        memoryRect.x -= w;
        memoryRect.width = size.x;
        memoryLabelRect = memoryRect;
        memoryLabelRect.x += size.x;
        memoryLabelRect.width = w - size.x;
      }

      sceneRect = memoryRect;
      if (showScene)
      {
        tempContent.text = sample.GetSceneName();
        w = currentLogStyle.CalcSize(tempContent).x + size.x;
        sceneRect.x -= w;
        sceneRect.width = size.x;
        sceneLabelRect = sceneRect;
        sceneLabelRect.x += size.x;
        sceneLabelRect.width = w - size.x;
      }

      timeRect = sceneRect;
      if (showTime)
      {
        tempContent.text = sample.time.ToString("0.000");
        w = currentLogStyle.CalcSize(tempContent).x + size.x;
        timeRect.x -= w;
        timeRect.width = size.x;
        timeLabelRect = timeRect;
        timeLabelRect.x += size.x;
        timeLabelRect.width = w - size.x;
      }


      GUILayout.BeginHorizontal(currentLogStyle);
      if (log == selectedLog)
      {
        GUILayout.Box(content, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
        GUILayout.Label(log.condition, selectedLogFontStyle);
        //GUILayout.FlexibleSpace();
        if (showTime)
        {
          GUI.Box(timeRect, showTimeContent, currentLogStyle);
          GUI.Label(timeLabelRect, sample.time.ToString("0.000"), currentLogStyle);
        }

        if (showScene)
        {
          GUI.Box(sceneRect, showSceneContent, currentLogStyle);
          GUI.Label(sceneLabelRect, sample.GetSceneName(), currentLogStyle);
        }

        if (showMemory)
        {
          GUI.Box(memoryRect, showMemoryContent, currentLogStyle);
          GUI.Label(memoryLabelRect, sample.memory.ToString("0.000") + " mb", currentLogStyle);
        }

        if (showFps)
        {
          GUI.Box(fpsRect, showFpsContent, currentLogStyle);
          GUI.Label(fpsLabelRect, sample.fpsText, currentLogStyle);
        }
      }
      else
      {
        if (GUILayout.Button(content, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y)))
        {
          //selectedIndex = startIndex + index ;
          selectedLog = log;
        }

        if (GUILayout.Button(log.condition, logButtonStyle))
        {
          //selectedIndex = startIndex + index ;
          selectedLog = log;
        }

        //GUILayout.FlexibleSpace();
        if (showTime)
        {
          GUI.Box(timeRect, showTimeContent, currentLogStyle);
          GUI.Label(timeLabelRect, sample.time.ToString("0.000"), currentLogStyle);
        }

        if (showScene)
        {
          GUI.Box(sceneRect, showSceneContent, currentLogStyle);
          GUI.Label(sceneLabelRect, sample.GetSceneName(), currentLogStyle);
        }

        if (showMemory)
        {
          GUI.Box(memoryRect, showMemoryContent, currentLogStyle);
          GUI.Label(memoryLabelRect, sample.memory.ToString("0.000") + " mb", currentLogStyle);
        }

        if (showFps)
        {
          GUI.Box(fpsRect, showFpsContent, currentLogStyle);
          GUI.Label(fpsLabelRect, sample.fpsText, currentLogStyle);
        }
      }

      if (collapse)
        GUI.Label(CountRect, log.count.ToString(), barStyle);
      GUILayout.EndHorizontal();
      index++;
    }

    int afterHeight = (int) ((totalCount - (startIndex + totalVisibleCount)) * size.y);
    if (afterHeight > 0)
    {
      //fill invisible gap after scroller to make proper scroller pos
      GUILayout.BeginHorizontal(GUILayout.Height(afterHeight));
      GUILayout.Label(" ");
      GUILayout.EndHorizontal();
    }

    GUILayout.EndScrollView();
    GUILayout.EndArea();

    ButtomRect.x = 0f;
    ButtomRect.y = Screen.height - size.y;
    ButtomRect.width = Screen.width;
    ButtomRect.height = size.y;

    if (showGraph)
      drawGraph();
    else
      drawStack();
  }


  private float graphSize = 4f;
  private int startFrame;
  private int currentFrame;
  private Vector3 tempVector1;
  private Vector3 tempVector2;
  private Vector2 graphScrollerPos;
  private float maxFpsValue;
  private float minFpsValue;
  private float maxMemoryValue;
  private float minMemoryValue;

  void drawGraph()
  {
    graphRect = stackRect;
    graphRect.height = Screen.height * 0.25f; //- size.y ;


    //startFrame = samples.Count - (int)(Screen.width / graphSize) ;
    //if( startFrame < 0 ) startFrame = 0 ;
    GUI.skin = graphScrollerSkin;

    Vector2 drag = getDrag();
    if (graphRect.Contains(new Vector2(downPos.x, Screen.height - downPos.y)))
    {
      if (Math.Abs(drag.x) > .01f)
      {
        graphScrollerPos.x -= drag.x - oldDrag3;
        graphScrollerPos.x = Mathf.Max(0, graphScrollerPos.x);
      }

      Vector2 p = downPos;
      if (p != Vector2.zero)
      {
        currentFrame = startFrame + (int) (p.x / graphSize);
      }
    }

    oldDrag3 = drag.x;
    GUILayout.BeginArea(graphRect, backStyle);

    graphScrollerPos = GUILayout.BeginScrollView(graphScrollerPos);
    startFrame = (int) (graphScrollerPos.x / graphSize);
    if (graphScrollerPos.x >= (samples.Count * graphSize - Screen.width))
      graphScrollerPos.x += graphSize;

    GUILayout.Label(" ", GUILayout.Width(samples.Count * graphSize));
    GUILayout.EndScrollView();
    GUILayout.EndArea();
    maxFpsValue = 0;
    minFpsValue = 100000;
    maxMemoryValue = 0;
    minMemoryValue = 100000;
    for (int i = 0; i < Screen.width / graphSize; i++)
    {
      int index = startFrame + i;
      if (index >= samples.Count)
        break;
      Sample s = samples[index];
      if (maxFpsValue < s.fps) maxFpsValue = s.fps;
      if (minFpsValue > s.fps) minFpsValue = s.fps;
      if (maxMemoryValue < s.memory) maxMemoryValue = s.memory;
      if (minMemoryValue > s.memory) minMemoryValue = s.memory;
    }

    //GUI.BeginGroup(graphRect);


    if (currentFrame != -1 && currentFrame < samples.Count)
    {
      Sample selectedSample = samples[currentFrame];
      GUILayout.BeginArea(ButtomRect, backStyle);
      GUILayout.BeginHorizontal();

      GUILayout.Box(showTimeContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
      GUILayout.Label(selectedSample.time.ToString("0.0"), nonStyle);
      GUILayout.Space(size.x);

      GUILayout.Box(showSceneContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
      GUILayout.Label(selectedSample.GetSceneName(), nonStyle);
      GUILayout.Space(size.x);

      GUILayout.Box(showMemoryContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
      GUILayout.Label(selectedSample.memory.ToString("0.000"), nonStyle);
      GUILayout.Space(size.x);

      GUILayout.Box(showFpsContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
      GUILayout.Label(selectedSample.fpsText, nonStyle);
      GUILayout.Space(size.x);

      /*GUILayout.Box( graphContent ,nonStyle, GUILayout.Width(size.x) ,GUILayout.Height(size.y));
      GUILayout.Label( currentFrame.ToString() ,nonStyle  );*/
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.EndArea();
    }

    graphMaxRect = stackRect;
    graphMaxRect.height = size.y;
    GUILayout.BeginArea(graphMaxRect);
    GUILayout.BeginHorizontal();

    GUILayout.Box(showMemoryContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Label(maxMemoryValue.ToString("0.000"), nonStyle);

    GUILayout.Box(showFpsContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
    GUILayout.Label(maxFpsValue.ToString("0.000"), nonStyle);
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();
    GUILayout.EndArea();

    graphMinRect = stackRect;
    graphMinRect.y = stackRect.y + stackRect.height - size.y;
    graphMinRect.height = size.y;
    GUILayout.BeginArea(graphMinRect);
    GUILayout.BeginHorizontal();

    GUILayout.Box(showMemoryContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));

    GUILayout.Label(minMemoryValue.ToString("0.000"), nonStyle);


    GUILayout.Box(showFpsContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));

    GUILayout.Label(minFpsValue.ToString("0.000"), nonStyle);
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();
    GUILayout.EndArea();

    //GUI.EndGroup();
  }

  void drawStack()
  {
    if (selectedLog != null)
    {
      Vector2 drag = getDrag();
      if (Math.Abs(drag.y) > .01f && stackRect.Contains(new Vector2(downPos.x, Screen.height - downPos.y)))
      {
        scrollPosition2.y += drag.y - oldDrag2;
      }

      oldDrag2 = drag.y;


      GUILayout.BeginArea(stackRect, backStyle);
      scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2);
      Sample selectedSample = null;
      try
      {
        selectedSample = samples[selectedLog.sampleId];
      }
      catch (Exception e)
      {
        Debug.LogException(e);
      }

      GUILayout.BeginHorizontal();
      GUILayout.Label(selectedLog.condition, stackLabelStyle);
      GUILayout.EndHorizontal();
      GUILayout.Space(size.y * 0.25f);
      GUILayout.BeginHorizontal();
      GUILayout.Label(selectedLog.stacktrace, stackLabelStyle);
      GUILayout.EndHorizontal();
      GUILayout.Space(size.y);
      GUILayout.EndScrollView();
      GUILayout.EndArea();


      GUILayout.BeginArea(ButtomRect, backStyle);
      GUILayout.BeginHorizontal();

      GUILayout.Box(showTimeContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
      if (selectedSample != null)
      {
        GUILayout.Label(selectedSample.time.ToString("0.000"), nonStyle);
        GUILayout.Space(size.x);

        GUILayout.Box(showSceneContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
        GUILayout.Label(selectedSample.GetSceneName(), nonStyle);
        GUILayout.Space(size.x);

        GUILayout.Box(showMemoryContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
        GUILayout.Label(selectedSample.memory.ToString("0.000"), nonStyle);
        GUILayout.Space(size.x);

        GUILayout.Box(showFpsContent, nonStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
        GUILayout.Label(selectedSample.fpsText, nonStyle);
      }

      /*GUILayout.Space( size.x );
      GUILayout.Box( graphContent ,nonStyle, GUILayout.Width(size.x) ,GUILayout.Height(size.y));
      GUILayout.Label( selectedLog.sampleId.ToString() ,nonStyle  );*/
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.EndArea();
    }
    else
    {
      GUILayout.BeginArea(stackRect, backStyle);
      GUILayout.EndArea();
      GUILayout.BeginArea(ButtomRect, backStyle);
      GUILayout.EndArea();
    }
  }


  public void OnGUIDraw()
  {
    if (!show)
    {
      return;
    }

    ScreenRect.x = 0;
    ScreenRect.y = 0;
    ScreenRect.width = Screen.width;
    ScreenRect.height = Screen.height;

    getDownPos();


    logsRect.x = 0f;
    logsRect.y = size.y * 2f;
    logsRect.width = Screen.width;
    logsRect.height = Screen.height * 0.75f - size.y * 2f;

    stackRect.x = 0f;
    stackRect.y = Screen.height * 0.75f;
    stackRect.width = Screen.width;
    stackRect.height = Screen.height * 0.25f - size.y;


    DetailRect.x = 0f;
    DetailRect.y = Screen.height - size.y * 3;
    DetailRect.width = Screen.width;
    DetailRect.height = size.y * 3;

    if (currentView == ReportView.Info)
      DrawInfo();
    else if (currentView == ReportView.Logs)
    {
      drawToolBar();
      DrawLogs();
    }
  }

  private readonly List<Vector2> gestureDetector = new List<Vector2>();
  Vector2 gestureSum = Vector2.zero;
  float gestureLength;
  int gestureCount;

  bool isGestureDone()
  {
    if (Application.platform == RuntimePlatform.Android ||
        Application.platform == RuntimePlatform.IPhonePlayer)
    {
      if (Input.touches.Length != 1)
      {
        gestureDetector.Clear();
        gestureCount = 0;
      }
      else
      {
        if (Input.touches[0].phase == TouchPhase.Canceled || Input.touches[0].phase == TouchPhase.Ended)
          gestureDetector.Clear();
        else if (Input.touches[0].phase == TouchPhase.Moved)
        {
          Vector2 p = Input.touches[0].position;
          if (gestureDetector.Count == 0 || (p - gestureDetector[gestureDetector.Count - 1]).magnitude > 10)
            gestureDetector.Add(p);
        }
      }
    }
    else
    {
      if (Input.GetMouseButtonUp(0))
      {
        gestureDetector.Clear();
        gestureCount = 0;
      }
      else
      {
        if (Input.GetMouseButton(0))
        {
          Vector2 p = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
          if (gestureDetector.Count == 0 || (p - gestureDetector[gestureDetector.Count - 1]).magnitude > 10)
            gestureDetector.Add(p);
        }
      }
    }

    if (gestureDetector.Count < 10)
      return false;

    gestureSum = Vector2.zero;
    gestureLength = 0;
    Vector2 prevDelta = Vector2.zero;
    for (int i = 0; i < gestureDetector.Count - 2; i++)
    {
      Vector2 delta = gestureDetector[i + 1] - gestureDetector[i];
      float deltaLength = delta.magnitude;
      gestureSum += delta;
      gestureLength += deltaLength;

      float dot = Vector2.Dot(delta, prevDelta);
      if (dot < 0f)
      {
        gestureDetector.Clear();
        gestureCount = 0;
        return false;
      }

      prevDelta = delta;
    }

    int gestureBase = (Screen.width + Screen.height) / 4;

    if (gestureLength > gestureBase && gestureSum.magnitude < (float) gestureBase / 2)
    {
      gestureDetector.Clear();
      gestureCount++;
      if (gestureCount >= numOfCircleToShow)
        return true;
    }

    return false;
  }

//  float lastClickTime = -1;

  //calculate  pos of first click on screen
  private Vector2 startPos;

  private Vector2 downPos;

  private void getDownPos()
  {
    if (Application.platform == RuntimePlatform.Android ||
        Application.platform == RuntimePlatform.IPhonePlayer)
    {
      if (Input.touches.Length == 1 && Input.touches[0].phase == TouchPhase.Began)
      {
        downPos = Input.touches[0].position;
//        return downPos;
      }
    }
    else
    {
      if (Input.GetMouseButtonDown(0))
      {
        downPos.x = Input.mousePosition.x;
        downPos.y = Input.mousePosition.y;
//        return downPos;
      }
    }

//    return Vector2.zero;
  }
  //calculate drag amount , this is used for scrolling

  Vector2 mousePosition;

  Vector2 getDrag()
  {
    if (Application.platform == RuntimePlatform.Android ||
        Application.platform == RuntimePlatform.IPhonePlayer)
    {
      if (Input.touches.Length != 1)
      {
        return Vector2.zero;
      }

      return Input.touches[0].position - downPos;
    }
    else
    {
      if (Input.GetMouseButton(0))
      {
        mousePosition = Input.mousePosition;
        return mousePosition - downPos;
      }
      else
      {
        return Vector2.zero;
      }
    }
  }

  //calculate the start index of visible log
  private void calculateStartIndex()
  {
    startIndex = (int) (scrollPosition.y / size.y);
    startIndex = Mathf.Clamp(startIndex, 0, currentLog.Count);
  }

  // For FPS Counter
  private int frames;
  private bool firstTime = true;
  private float lastUpdate;
  private const int requiredFrames = 10;
  private const float updateInterval = 0.25f;

#if UNITY_CHANGE1
	private float lastUpdate2;
#endif

  private void doShow()
  {
    show = true;
    currentView = ReportView.Logs;
    gameObject.AddComponent<ReporterGUI>();


    try
    {
      gameObject.SendMessage("OnShowReporter");
    }
    catch (Exception e)
    {
      Debug.LogException(e);
    }
  }

  private void Update()
  {
    fpsText = fps.ToString("0.000");
    gcTotalMemory = (((float) GC.GetTotalMemory(false)) / 1024 / 1024);
    //addSample();

#if UNITY_CHANGE3
    int sceneIndex = SceneManager.GetActiveScene().buildIndex;
    if (sceneIndex != -1 && string.IsNullOrEmpty(scenes[sceneIndex]))
      scenes[SceneManager.GetActiveScene().buildIndex] = SceneManager.GetActiveScene().name;
#else
		int sceneIndex = Application.loadedLevel;
		if (sceneIndex != -1 && string.IsNullOrEmpty(scenes[Application.loadedLevel]))
			scenes[Application.loadedLevel] = Application.loadedLevelName;
#endif

    calculateStartIndex();
    if (!show && isGestureDone())
    {
      doShow();
    }


    lock (threadedLogs)
    {
      if (threadedLogs.Count > 0)
      {
        lock (threadedLogs)
        {
          foreach (var l in threadedLogs)
          {
            AddLog(l.condition, l.stacktrace, (LogType) l.logType);
          }

          threadedLogs.Clear();
        }
      }
    }

#if UNITY_CHANGE1
		float elapsed2 = Time.realtimeSinceStartup - lastUpdate2;
		if (elapsed2 > 1) {
			lastUpdate2 = Time.realtimeSinceStartup;
			//be sure no body else take control of log
			Application.RegisterLogCallback (new Application.LogCallback (CaptureLog));
			Application.RegisterLogCallbackThreaded (new Application.LogCallback (CaptureLogThread));
		}
#endif

    // FPS Counter
    if (firstTime)
    {
      firstTime = false;
      lastUpdate = Time.realtimeSinceStartup;
      frames = 0;
      return;
    }

    frames++;
    float dt = Time.realtimeSinceStartup - lastUpdate;
    if (dt > updateInterval && frames > requiredFrames)
    {
      fps = frames / dt;
      lastUpdate = Time.realtimeSinceStartup;
      frames = 0;
    }
  }

  private void AddLog(string condition, string stacktrace, LogType type)
  {
    float memUsage = 0f;
    string _condition;
    if (cachedString.ContainsKey(condition))
    {
      _condition = cachedString[condition];
    }
    else
    {
      _condition = condition;
      cachedString.Add(_condition, _condition);
      memUsage += (string.IsNullOrEmpty(_condition) ? 0 : _condition.Length * sizeof(char));
      memUsage += IntPtr.Size;
    }

    string _stacktrace;
    if (cachedString.ContainsKey(stacktrace))
    {
      _stacktrace = cachedString[stacktrace];
    }
    else
    {
      _stacktrace = stacktrace;
      cachedString.Add(_stacktrace, _stacktrace);
      memUsage += (string.IsNullOrEmpty(_stacktrace) ? 0 : _stacktrace.Length * sizeof(char));
      memUsage += IntPtr.Size;
    }

    bool newLogAdded = false;

    addSample();
    Log log = new Log()
    {
      logType = (_LogType) type,
      condition = _condition,
      stacktrace = _stacktrace,
      sampleId = samples.Count - 1
    };
    memUsage += log.GetMemoryUsage();
    //memUsage += samples.Count * 13 ;

    logsMemUsage += memUsage / 1024 / 1024;

    if (TotalMemUsage > maxSize)
    {
      clear();
      Debug.Log("Memory Usage Reach" + maxSize + " mb So It is Cleared");
      return;
    }

    bool isNew = false;
    //string key = _condition;// + "_!_" + _stacktrace ;
    if (logsDic.ContainsKey(_condition, stacktrace))
    {
      logsDic[_condition][stacktrace].count++;
    }
    else
    {
      isNew = true;
      collapsedLogs.Add(log);
      logsDic[_condition][stacktrace] = log;

      if (type == LogType.Log)
        numOfCollapsedLogs++;
      else if (type == LogType.Warning)
        numOfCollapsedLogsWarning++;
      else
        numOfCollapsedLogsError++;
    }

    if (type == LogType.Log)
      numOfLogs++;
    else if (type == LogType.Warning)
      numOfLogsWarning++;
    else
      numOfLogsError++;


    logs.Add(log);
    if (!collapse || isNew)
    {
      bool skip = log.logType == _LogType.Log && !showLog;
      if (log.logType == _LogType.Warning && !showWarning)
        skip = true;
      if (log.logType == _LogType.Error && !showError)
        skip = true;
      if (log.logType == _LogType.Assert && !showError)
        skip = true;
      if (log.logType == _LogType.Exception && !showError)
        skip = true;

      if (!skip)
      {
        if (string.IsNullOrEmpty(filterText) || log.condition.ToLower().Contains(filterText.ToLower()))
        {
          currentLog.Add(log);
          newLogAdded = true;
        }
      }
    }

    if (newLogAdded)
    {
      calculateStartIndex();
      int totalCount = currentLog.Count;
      int totalVisibleCount = (int) (Screen.height * 0.75f / size.y);
      if (startIndex >= (totalCount - totalVisibleCount))
        scrollPosition.y += size.y;
    }

    try
    {
      gameObject.SendMessage("OnLog", log);
    }
    catch (Exception e)
    {
      Debug.LogException(e);
    }
  }

  private readonly List<Log> threadedLogs = new List<Log>();

  private void CaptureLogThread(string condition, string stacktrace, LogType type)
  {
    Log log = new Log() {condition = condition, stacktrace = stacktrace, logType = (_LogType) type};
    lock (threadedLogs)
    {
      threadedLogs.Add(log);
    }
  }

  //new scene is loaded
//	void OnLevelWasLoaded()
//	{
//		if (clearOnNewSceneLoaded)
//			clear();

//#if UNITY_CHANGE3
//		currentScene = SceneManager.GetActiveScene().name ;
//		Debug.Log( "Scene " + SceneManager.GetActiveScene().name + " is loaded");
//#else
//		currentScene = Application.loadedLevelName;
//		Debug.Log("Scene " + Application.loadedLevelName + " is loaded");
//#endif
//	}

  //save user config
  private void OnApplicationQuit()
  {
    PlayerPrefs.SetInt("Reporter_currentView", (int) currentView);
    PlayerPrefs.SetInt("Reporter_show", (show) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_collapse", (collapse) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_clearOnNewSceneLoaded", (clearOnNewSceneLoaded) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showTime", (showTime) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showScene", (showScene) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showMemory", (showMemory) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showFps", (showFps) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showGraph", (showGraph) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showLog", (showLog) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showWarning", (showWarning) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showError", (showError) ? 1 : 0);
    PlayerPrefs.SetString("Reporter_filterText", filterText);
    PlayerPrefs.SetFloat("Reporter_size", size.x);

    PlayerPrefs.SetInt("Reporter_showClearOnNewSceneLoadedButton", (showClearOnNewSceneLoadedButton) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showTimeButton", (showTimeButton) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showSceneButton", (showSceneButton) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showMemButton", (showMemButton) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showFpsButton", (showFpsButton) ? 1 : 0);
    PlayerPrefs.SetInt("Reporter_showSearchText", (showSearchText) ? 1 : 0);

    PlayerPrefs.Save();
  }

  //read build information
  IEnumerator readInfo()
  {
    string prefFile = "build_info.txt";
    string url = prefFile;

    if (prefFile.IndexOf("://", StringComparison.Ordinal) == -1)
    {
      string streamingAssetsPath = Application.streamingAssetsPath;
      if (streamingAssetsPath == "")
        streamingAssetsPath = Application.dataPath + "/StreamingAssets/";
      url = System.IO.Path.Combine(streamingAssetsPath, prefFile);
    }

    if (Application.platform != RuntimePlatform.WebGLPlayer)
      if (!url.Contains("://"))
        url = "file://" + url;


    // float startTime = Time.realtimeSinceStartup;
    WWW www = new WWW(url);
    yield return www;

    if (!string.IsNullOrEmpty(www.error))
    {
      Debug.LogError(www.error);
    }
    else
    {
      buildDate = www.text;
    }
  }
}