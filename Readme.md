# MonoCrome

MonoCrome это - высокоуровневый компонентно-ориентированный Framework для MonoGame. На данный момент реализующий самый базовый функционал необходимый для создания 2D игры, без необходимости писать какой-либо код помимо логики ваших компонентов. Пример работы с Framework'ом: [Asteroids](https://github.com/goldsphere48/Asteroids).

# Мотивация
Этот проект является учёбным и сделан с целью развития практических навыков программирования на C# и в целом усвоения теории программирования, применении паттернов и попытка написать грамотный код. Пока что проект на стадии прототипа и многие моменты необходимо подвргнуть рефакторингу, однако цель - реализовать Unity подобный подход к созданию игр на MonoGame, считаю достигнут.
В процессе разработки я ориентровался на API Unity, иногда повторяя даже имена методов. Однако на структуру DI контейнера компонентов меня вдохновил [данный](https://github.com/jhauberg/ComponentKit) Framework. 

# Features!

 1) Набор базовых компонентов : 
  - Transform - отвечает за положение объекта в пространстве, а также организацию иерархии объектов на сцене
  - BoxCollider2D - создаёт границу объекту соответсвующую Render компоненту, либо задаёт кастомню границу. Служит для проверки пересечения с другими Collider'ами, а также для проверки нажатия по объекту.
  - TextRenderer - рендер текста на сцене
  - SpriteRenderer - рендер спрайта на сцене
  - DebugRenderer - рендер границ Collider'a
2) Поддержка иерархии GameObject'ов на подобии того как это сделано в Unity
3) Система управления сценами
4) Аттрибуты InsertComponent и InsertGameObject, прокидывающие зависимые компоненты и объекты через механизм рефлексии, что делает код значительно чище и проще.
5) Система кэширования компонентов и методов оптимизирует работу Framework'a, кэшируя необхоимые компоненты, такие как Renderer, Collider и методы Update, Awake, Start и.т.д. тем самым исключая лишние вызовы.
6) LayerManager. Позволяет управлять слоями.
7) IPointerClickHandler - интерфейс добавляющий метод, срабатывающйи при клике на Collider объекта и несколько других интерфейсов, описанных в блоке Input выше.
8) Entity - Система порождения игровых объектов

Всё это более подробно будет описано далее.

# Объекты и компоненты
## GameObject
Отражением игрового объекта на сцене является класс GameObject. Он же является условным контейнеров компонентов, которые задают поведение игрового объекта. Каждый GameObject по умолчанию имеет компонент Transform.
```sh
 public Transform Transform { get; internal set; }
```
а также содержит интерфейс для добавления / удаления / получения компонентов оьбъекта:
```sh
public void AddComponent(Type componentType)
public void AddComponent(Component component)
public void AddComponent<T>() where T : Component
public Component GetComponent(Type componentType, bool inherit = false)
public T GetComponent<T>(bool inherit = false) where T : Component
public Component GetComponentInChildren(Type componentType, bool inherit = false)
public T GetComponentInChildren<T>(bool inherit = false) where T : Component
public Component GetComponentInParent(Type componentType, bool inherit = false)
public T GetComponentInParent<T>(bool inherit = false) where T : Component
public IEnumerable<Component> GetComponents(Type componentType, bool inherit = true)
public IEnumerable<T> GetComponents<T>(bool inherit = true) where T : Component
public IEnumerable<Component> GetComponents()
public IEnumerable<Component> GetComponentsInChildren(Type componentType, bool inherit = false)
public IEnumerable<T> GetComponentsInChildren<T>(bool inherit = false) where T : Component
public IEnumerable<Component> GetComponentsInParent(Type componentType, bool inherit = false)
public IEnumerable<T> GetComponentsInParent<T>(bool inherit = false) where T : Component
public bool HasComponent<T>(bool inherit = false) where T : Component
public void RemoveComponent(Type type)
```

Методы добавления объекта на сцену / удаления объекта со сцены:
```sh
public static void Destroy(GameObject gameObject)
public static void Instatiate(GameObject gameObject)
public static void Instatiate(GameObject gameObject, string layerName)
public static void Instatiate(GameObject gameObject, DefaultLayers layer)
```
Публичные свойства
```sh
public string LayerName { get; }
public string Name { get; }
public int ZIndex {get;set;}
public bool Enabled {get;set;}
public Transform Transform { get; }
```

### Создание объектов:
```sh
var go = Entity.Create("Go1", new Component1(), <Список компонентов через запятую>)
```
### Методы порождения объектов:
```sh
public static GameObject Create(params Component[] components)
public static GameObject Create(string name, params Component[] components)
public static GameObject CreateFromDefinition(string definition, string name)
```

Последний метод использует дефиниции (заранее заготовленную схему объекта, состоящую из набора типов компонентов, составляющих объект).
Пример создания дефиниции:
```sh
Entity.Define("Name", typeof(Component1), typeof(Component2), ...);
```

### Методы определения дефиниций:
```sh
public static void Define(string definition, params Type[] componentTypes)
public static void Define(string definition, string inheritFromDefinition, params Type[] componentTypes)
```

### Композиция объектов:
Есть два способа организации иерархии объектов
1) Через присвание Transform.Parent'у родительского объекта:
```sh
go1.Transform.Parent = go2.Transform;
```
Декопозировать можно так:
```sh
go1.Transform.Parent = null;
```
2) Через интерфейс Entity:
```sh
//Присвает результат композиции объекту parent
public static GameObject Compose(GameObject parent, params GameObject[] childrens
public static GameObject Compose(string name, params GameObject[] childrens)
//Пораждает новый объект с дочерними элементами childrens
public static GameObject ComposeNew(params GameObject[] childrens)
public static GameObject ComposeNew(string name, params GameObject[] childrens)
```
Пример:
```sh
var b = Entity.Create();
var a = Entity.ComposeNew("Go1", 
    Entity.Create(),
    Entity.ComposeNew(b, Entity.Create())
);
```
Метод
```sh
public static IEnumerable<GameObject> Decompose(GameObject gameObject)
```
полностью линеаризует объект.

## Компоненты

Компонентом называется класс наследник MonoCrome.Core.Component. Каждый компонент имеет ссылку на объект к которому приклеплён. Повторяет интерфейс GameObject'а для доступа к компонентам объекта. А также имеет неявные методы:
- Awake - вызывается перед добавлением на сцену
- Start - вызывается после добавлением на сцену
- Update - вызывается каждый фрейм
- OnEnable - вызывается при включении компонента (если он не был включён)
- OnDisable - вызывается при выключении компонента (если он не был выключен)
- OnFinalize - вызывается при удалении объекта и нужен для очистки неуправляемых ресурсов компонента 
- OnDestroy- вызывается при удалении объекта и нужен для очистки управляемых ресурсов компонента 
- OnCollision(Collision collision)- вызывается при пересечении Collider'a объекта к которму прикреплён компонент с другим Collider'ом. (Работает только если есть Collider)
данные методы компонент содержим неявно (как в Unity, если их объявить они будут существовать в компоенте и закэшируются системой, если нет, но никто их лишний раз даже не вызовёт). Пример компонента:

```sh
class ComponentA : Component
{
    private Transform _transform;
    private void Start()
    {
        _transform = GetComponent<Transform>();
    }
    
    private void Update()
    {
        _transform.Position += new Vector2(5);
    }
}
```

## Аттрибуты InsertComponent и InsertGameObject
### InsertComponent
```sh
public string From { get; set; }
public bool Inherit { get; set; } = false;
public bool Required { get; set; } = false;
public void AcceptFieldVisitor(FieldAttributeVisitor visitor);
```

Данный атррибут присваивает помеченому полю указанный компонент из объекта с имненем From
```sh
[InsertComponent(From = "StarShip")] private Health _health;
```
Если свойство From не указано, то компонент ищется среди компонентов текущего GameObject'a
```sh
[InsertComponent] private Health _health;
```
Если флаг Required установлен в true и при добавлении объекта на сцену, компонент не был найден, будет выброшено исключение. 
```sh
[InsertComponent(Required = true)] private Health _health;
```
Если флаг Inherit установлен в true, то в качестве типа поля можно указать базовый тип и система подставит первый подходящий компонент данного типа либо наследника данного типа (в том числе можно использовать тип интерфейса).
```sh
//подставит SpriteRenderer
[InsertComponent(Inherit = true)] private Renderer _renderer;
[InsertComponent(Inherit = true, From = "Boss", Required = true)] private IDamagable _damagable;
```
### InsertGameObject
Данный аттрибут ведёт себя также как и InsertComponent, только в качестве типа использует GameObject.
```sh
//подставит SpriteRenderer
[InsertGameObject("StarShip", Required = true)] private GameObject _ship;
```
### Особености аттрибутов
Данные аттрибуты поддерживают перекрёстные ссылки, а также способны "ожидать" пока необходимый компонент не будет добавлен (если Required = false). То есть как-только нужный компонент / объект будет добавлен на сцену (это может произойти когда угодно), он сразу попадёт в поле помеченнное аттрибутом, его ожидающем. Также важной особенностью является то, что если компонент / объект были удалены со сцены, то в поле будет возвращён null.

# Сцены
Сценой называется объект - наследник класса MonoChrome.SceneSystem.Scene
```sh
public ContentManager Content { get;}
public Game Game { get; }
public GraphicsDevice GraphicsDevice { get; }
protected LayerManager LayerManager { get;}
public void Destroy(GameObject gameObject)
public void Instatiate(GameObject gameObject, string layerName)
public void Instatiate(GameObject gameObject, DefaultLayers layerName)
public void Instatiate(GameObject gameObject)
public virtual void OnDisable()
public virtual void OnEnable()
public abstract void Setup();
```
Scene имеется единственый абстрактный метод Setup предназначенный для настройки сцены и заполнении её объектами. А также настройки самих объектов (хотя это можно делегировать другим классам, например создав фабрику ActorFactory, и определив в ней методы порождающие объекты с необходимыми наборами компонентов).
Пример сцены: 
```sh
internal class MainMenuScene : Scene
{
    private Vector2 Window => new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth,
           GraphicsDevice.PresentationParameters.BackBufferHeight);

    private GameObject _startButton;
    private GameObject _exitButton;

    public override void Setup()
    {
        _startButton = CreateCenterButton("play", new Vector2(0, -50), OnStartClick);
        _exitButton = CreateCenterButton("exit", new Vector2(0, 50), () => Game.Exit());
    }

    private GameObject CreateCenterButton(string name, Vector2 position, Action onClick)
    {
        var button = ActorFactory.CreateButton(name, name);
        Instatiate(button, DefaultLayers.UI);
        var box = button.GetComponent<BoxCollider2D>().Bounds;
        button.Transform.Origin = new Vector2(box.Size.X / 2, box.Size.Y / 2);
        button.GetComponent<ButtonController>().OnClick = onClick;
        button.Transform.Position = new Vector2(Window.X / 2 + position.X, Window.Y / 2 + position.Y);
        return button;
    }

    private void OnStartClick()
    {
        SceneManager.Instance.LoadScene<GameScene>();
        SceneManager.Instance.SetActiveScene<GameScene>();
    }
}
```
 ### SceneManager
 Для управления сценами (добавления / удаления / загрузки / переходы) предназанчен синглтон SceneManager.Instance
 ```sh
 public ContentManager Content { get; set; }
public Scene CurrentScene {get;}
public Game Game { get; set; }
public GraphicsDevice GraphicsDevice { get; set; }
public void Clear()
public void ClearAllExceptCurrent()
public void Draw()
public bool IsLoaded(Type type)
public bool IsLoaded<T>() where T : IScene
public void LoadScene(Type type)
public void LoadScene<T>() where T : IScene
public void SetActiveScene(Type type)
public void SetActiveScene<T>() where T : IScene
public void UnloadScene(Type type)
public void UnloadScene<T>() where T : IScene
public void Update(GameTime gameTime)
 ```
 Посредством методов Update / Draw SceneManager интегрируется в жизненый цикл игры следующим образом
```sh
public class AsteroidGame : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    public AsteroidGame()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        ActorFactory.Content = Content;
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        SceneManager.Instance.GraphicsDevice = graphics.GraphicsDevice;
        SceneManager.Instance.Content = Content;
        SceneManager.Instance.Game = this;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        SceneManager.Instance.LoadScene<MainMenuScene>();
        SceneManager.Instance.SetActiveScene<MainMenuScene>();
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void UnloadContent()
    {
        SceneManager.Instance.UnloadScene<MainMenuScene>();
    }

    protected override void Update(GameTime gameTime)
    {
        SceneManager.Instance.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        SceneManager.Instance.Draw();
        base.Draw(gameTime);
    }
}
```
 
 # LayerManager
 Все игровые объекты обязательно рсполагаются на слоях.Каждый слой имеет свой zIndex. При этом каждый объект в рамках своего слоя имеет отдельный zIndex. Он определят порядок отрисовки и обработки событий.
Каждый слой имеет свои настройки, такие как:
```sh
// Продолжить обрабатывать собтие ввода на следующих слоях
bool AllowThroughHandling { get; set; }
// Осуществлять ли проверку столкновений на слое
bool CollisionDetectionEnable { get; set; }
// Включение / отключение слоя (выключеный слой полностью отсутсвует на сцене)
bool Enabled { get; set; }
// Обрабатывать события ввыода на слое
bool HandleInput { get; set; }
// Имя слоя
string Name { get; }
// Отрисовавать ли слой
bool Visible { get; set; }
// Порядок слоя
int ZIndex { get; set; }
```
  Всего поумолчанию существует 4 слоя
  | Name                     | AllowThroughHandling | CollisionDetectionEnable | HandleInput | ZIndex      | Visible | Enable |
|--------------------------|----------------------|--------------------------|-------------|-------------|---------|--------|
| DefaultLayers.Background | false                | false                    | false       | -2147482648 | true    | true   |
| DefaultLayers.Default    | false                | true                     | true        | 0           | true    | true   |
| DefaultLayers.Foreground | false                | false                    | false       | 2147482647  | true    | true   |
| DefaultLayers.UI         | false                | false                    | true        | 2147483548  | true    | true   |


Но ничто не мешает создать свой слой и настроить его по своему, это возможно благодаря интерфейсу LayerManager
```sh
public Layer CreateLayer(string layerName, int zIndex)
public ILayerSettings GetLayer(string layerName)
public ILayerSettings GetLayer(DefaultLayers layerName)
public void SetZIndex(string layerName, int zIndex)
public void SetZIndex(DefaultLayers layerName, int zIndex)
```
Доступ к нему можно получить из сцены. А также внутри компонентов
```sh
ComponentA.Scene.LayerManager
```
## Input
Все стандартные средства ввода из MonoGame можно использовать внутри компонентов. Однако добаилось несколько более эффективных и удобных средств.
### IPointerClickHandler
```sh
void OnPointerClick(PointerEventData pointerEventData);
```

Добавляет метод срабатывающий при клике по Collider'у объекта, если он есть. В качестве аргумента используется структура PointerEventData:
```sh
public MouseButton Button { get; set; }
public Vector2 Position { get; set; }
```
### IMouseOverHandler
```sh
void OnMouseExit();
void OnMouseOver();
```
Метод OnMouseOver срабатыват при наведении мышки на Collider объекта каждый фрейм.
Метод OnMouseExit срабатыват при убирании мышки с Collider объекта единожды.
### IKeyboardHandler
```sh
void KeyboardHandle(KeyboardState state);
```
Срабатывает при нажатии клаваши на клавиатуре

## Собсвтенные Renderer'ы, Collider'ы
Вы моежете создать собственные компоненты для рендера компонентов и коллайдеры других форм путём наследования от классов GameObjectRenderer и Collider соответственно.

 # Итог
 Документация может не содержать всей информации, а также не быть окончательной и может содержать ошибки. Также как и API и реализация не являются окончательными. Есть идеи что можно улущить или вообще переписать, какие кмопоненты добавить и прочее, буду стараться обнолять данный Framework, но следует упомянуть что это лишь учёбный проект, созданный в личных целях и я не беру никаких обязательств по его регулярной поддержке.
 С замченаниями и пожеланиями сюда: [goldsphere48@gmail.com](mailto:goldsphere48@gmail.com?subject=[GitHub]MonoCrome)
