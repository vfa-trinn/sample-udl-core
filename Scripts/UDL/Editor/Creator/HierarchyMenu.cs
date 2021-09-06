using UDL.Core.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HierarchyMenu {

    [MenuItem("GameObject/UI/Customized/Scroll View", false)]
    public static void Foo()
    {
        var gameObject = Selection.activeGameObject;

        GameObject scrollView = new GameObject("Scroll View");
        RectTransform scrollViewRT = scrollView.AddComponent<RectTransform>();
        scrollView.transform.SetParent(gameObject.transform);

        scrollViewRT.anchoredPosition = new Vector2(0, 0);
        scrollViewRT.sizeDelta = new Vector2(500, 500);
        scrollViewRT.localScale = Vector3.one;


        GameObject viewPort = new GameObject("Viewport");
        RectTransform viewPortRT = viewPort.AddComponent<RectTransform>();
        viewPort.transform.SetParent(scrollView.transform);

        viewPortRT.anchorMin = new Vector2(0, 0);
        viewPortRT.anchorMax = new Vector2(1, 1);
        viewPortRT.anchoredPosition = new Vector2(0, 0);
        viewPortRT.sizeDelta = new Vector2(1, 1);
        viewPortRT.localScale = Vector3.one;

        GameObject content = new GameObject("Content");
        RectTransform contentRT = content.AddComponent<RectTransform>();
        content.transform.SetParent(viewPort.transform);

        contentRT.anchorMin = new Vector2(0, 0);
        contentRT.anchorMax = new Vector2(1, 1);
        contentRT.anchoredPosition = new Vector2(0, 0);
        contentRT.sizeDelta = new Vector2(1, 1);
        contentRT.pivot = new Vector2(0, 1);
        contentRT.localScale = Vector3.one;

        var scrollRect = scrollView.AddComponent<ScrollRect>();
        scrollRect.content = content.GetComponent<RectTransform>();
        scrollRect.viewport = viewPort.GetComponent<RectTransform>();
        scrollRect.horizontal = false;
        scrollRect.movementType = ScrollRect.MovementType.Elastic;
        scrollRect.elasticity = 0.1f;
        scrollRect.inertia = true;
        scrollRect.decelerationRate = 0.135f;
        scrollRect.scrollSensitivity = 1;

        viewPort.AddComponent<Image>();
        Mask viewPortMask = viewPort.AddComponent<Mask>();
        viewPortMask.showMaskGraphic = false;

        VerticalLayoutGroup layoutGroup = content.AddComponent<VerticalLayoutGroup>();
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = true;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.spacing = 10;
        layoutGroup.padding = new RectOffset(0, 0, 0, 200);

        ContentSizeFitter contentSizeFitter = content.AddComponent<ContentSizeFitter>();
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        CommonContainer commonContainer = content.AddComponent<CommonContainer>();

        GameObject elementPrefab = new GameObject("Element Prefab");
        RectTransform elementPrefabRT = elementPrefab.AddComponent<RectTransform>();
        elementPrefab.transform.SetParent(content.transform);
        elementPrefabRT.localScale = Vector3.one;
        Image elementImage = elementPrefab.AddComponent<Image>();
        Button elementButton = elementPrefab.AddComponent<Button>();
        elementButton.image = elementImage;
        LayoutElement layoutElement = elementPrefab.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = 100;

        commonContainer.prefabs = new GameObject[] { elementPrefab };
    }
}