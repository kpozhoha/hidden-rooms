using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;

public class CreatorIcons : EditorWindow
{
    private const string PATH_ITEMS = "Items Editor";
    private const string PATH_ICON = "Assets/Sprites/Items Icons";

    private VisualElement _containerImage;
    private Button _saveButton;
    private Toggle _backToggle;

    private Item _chooseItem;
    [SerializeField] private Vector2Int _size = new Vector2Int(256, 256);
    [SerializeField] private Vector3 _direction;
    [SerializeField] private bool _isBackground;


    [MenuItem("Editor/Creator Icon")]
    public static void OpenWindow()
    {
        CreatorIcons wnd = GetWindow<CreatorIcons>();
        wnd.titleContent = new GUIContent("CreatorIcons");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Image Tool/Editor/CreatorIcons.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Image Tool/Editor/CreatorIcons.uss");
        root.styleSheets.Add(styleSheet);

        var listView = root.Q<ListView>("items");
        _containerImage = root.Q<VisualElement>("image");
        _saveButton = root.Q<Button>("save-icon");
        _saveButton.clicked += SaveIcon;



        BindingProperty(root);
        FillListView(listView);

        listView.onSelectionChange += OnSelectionItem;
    }

    private void OnSelectionItem(IEnumerable<object> obj)
    {
        _chooseItem = obj.First() as Item;
        SetImage();
    }

    private void BindingProperty(VisualElement root)
    {
        var sizeView = root.Q<Vector2IntField>("size");
        Vector3Field directionView = root.Q<Vector3Field>("direction");
        var toggle = root.Q<Toggle>("is-back");

        SerializedObject so = new SerializedObject(this);
        var sizeProp = so.FindProperty("_size");
        var directionProp = so.FindProperty("_direction");


        sizeView.bindingPath = "_size";
        directionView.bindingPath = "_direction";

        toggle.bindingPath = "_isBackground";

        root.Bind(so);
        directionView.RegisterCallback<ChangeEvent<Vector3>>(OnChangeDirection, TrickleDown.NoTrickleDown);
    }

    private void OnChangeDirection(ChangeEvent<Vector3> evt)
    {
        SetImage();
    }

    private void FillListView(ListView listView)
    {
        var allItems = Loader.LoadItems<Item>(PATH_ITEMS);

        listView.makeItem = () => new Label();
        listView.bindItem = (item, index) =>
        {
            (item as Label).text = allItems[index].name;
        };

        listView.itemsSource = allItems;
    }

    private void SetImage()
    {
        _containerImage.Clear();

        var image = new Image();
        image.scaleMode = ScaleMode.ScaleToFit;
        image.sprite = GetSprite();

        _containerImage.Add(image);
    }

    private Sprite GetSprite()
    {
        Texture2D texture = CreateTexture();
        var sprite = Sprite.Create(texture, new Rect(0, 0, _size.x, _size.y), Vector2.one / 2);
        return sprite;
    }

    private Texture2D CreateTexture()
    {
        RuntimePreviewGenerator.RenderSupersampling = 5;
        RuntimePreviewGenerator.MarkTextureNonReadable = false;
        RuntimePreviewGenerator.BackgroundColor = _isBackground ? new Color(0.1176471f, 0.05882353f, 0.1490196f,1)  : new Color(0, 0, 0, 0);
        RuntimePreviewGenerator.PreviewDirection = _direction;
        var texture = RuntimePreviewGenerator.GenerateModelPreview(_chooseItem.transform, _size.x, _size.y);
        return texture;
    }

    private void SaveIcon()
    {
        var texture = CreateTexture();
        byte[] picturePNG = texture.EncodeToPNG();
        string pathSprite = PATH_ICON + "/" +_chooseItem.gameObject.name + ".png";
        File.WriteAllBytes(@pathSprite, picturePNG);
        Debug.Log("Save");
    }
}