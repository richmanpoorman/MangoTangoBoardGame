using GdUnit4.Asserts;
using Godot;
using System;
using System.Collections.Generic;  


public partial class AssetSelection : Node2D
{    
    private struct AssetSelector {
        public AssetSelector(Button _popupButton, FileDialog _spriteChooser, CompressedTexture2D _defaultSprite, int _verticalOffset) {
            popupButton = _popupButton; 
            spriteChooser = _spriteChooser; 
            defaultSprite = _defaultSprite; 
            verticalOffset = _verticalOffset; 
        }

        public Button popupButton {set; get; }
        public FileDialog spriteChooser {set; get; }
        public CompressedTexture2D defaultSprite {set; get; }
        public int verticalOffset {set; get; }
    }
    [Export]
    private int maxStringLength = 30; 
    

    private static Vector2I gridSize = new Vector2I(16, 10);
    private EventBus _bus; 

    private Dictionary<string, AssetSelector> assetSelections = new Dictionary<string, AssetSelector>(); 
    private Dictionary<string, string> paths = new Dictionary<string, string>(); 

    [Export(PropertyHint.Enum, "P1_SCOUT,P1_TILE,P2_SCOUT,P2_TILE,EMPTY_SQUARE")]
    private string[] assetTypes;
    
    [Export]
    private Button[] popupButtons; 

    [Export] 
    private FileDialog[] spriteChoosers; 

    [Export]
    private CompressedTexture2D[] defaultSprites; 

    [Export]
    private SpinBox[] offsetSelections; 

    public override void _Ready() {
        _bus = EventBus.Bus; 

        int size = assetTypes.Length; 
        for (int i = 0; i < size; i++) 
            assetSelections[assetTypes[i]] = new AssetSelector(popupButtons[i], spriteChoosers[i], defaultSprites[i], (int)offsetSelections[i].Value); 

        foreach (var (assetType, assetSelector) in assetSelections) {
            assetSelector.popupButton.Pressed        += () => assetSelector.spriteChooser.Popup(); 
            assetSelector.spriteChooser.FileSelected += (string spritePath) => setPath(assetType, spritePath, assetSelector.popupButton); 
            setPath(assetType, "", assetSelector.popupButton);
        }
        
    }

    private void setPath(string assetType, string path, Button popupButton) {
        paths[assetType] = path; 
        if (path.Length > maxStringLength) path = "..." + path.Substring(path.Length - maxStringLength - 3); 
        popupButton.Text = "File: " + path; 
    }

    public void onSubmission() {

        (TileSet newTiles, Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<int, int>> newIDs) = createTileset(); 
        
        _bus.EmitSignal(EventBus.SignalName.onChangePieceTileset, newTiles, newIDs); 

        // Clears the buttons 
        foreach (var (assetType, assetSelector) in assetSelections) setPath(assetType, "", assetSelector.popupButton);
    }

    private TileSetSource getTileFromSource(string path, AssetSelector asset){
        TileSetAtlasSource spriteSheet = new TileSetAtlasSource();
        Texture2D sprites;
        if (path.Equals("")) {
            GD.Print("Using Default");
            sprites = asset.defaultSprite; 
        } else {
            Image image = new Image();
            Error didLoad = image.Load(path); 

            GD.Print("Trying to load: " + path);
            if (didLoad != Error.Ok) GD.Print("Image didn't load: " + path);
            else GD.Print("Succeeded in loading: " + path); 

            ImageTexture imageTexture = new ImageTexture();
            imageTexture.SetImage(image); 
            GD.Print("Has size: " + imageTexture.GetSize());
            sprites = imageTexture;
        }

        spriteSheet.TextureRegionSize = (Vector2I)sprites.GetSize(); 
        spriteSheet.Texture = sprites; 
        spriteSheet.CreateTile(Vector2I.Zero); 
        TileData data = spriteSheet.GetTileData(Vector2I.Zero, 0);
        data.TextureOrigin = new Vector2I(0, asset.verticalOffset); 
        
        return spriteSheet; 
    }
    // Note the dictionary is [color][type] but godot needs them to be ints instead of enums
    private (TileSet, Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<int, int>>) createTileset() {
        Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<int, int>> idMap = new Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<int, int>>();
        TileSet newSprites = new TileSet(); 
        newSprites.TileSize = gridSize;
        
        foreach (var (assetType, assetSelector) in assetSelections) {

            TileSetSource tile = getTileFromSource(paths[assetType], assetSelector);
            // return (newSprites, idMap); 
            int id = newSprites.AddSource(tile); 

            (PlayerColor assetColor, PieceType assetPieceType) = assetTypeStringToEnums(assetType);  
            if (!idMap.ContainsKey((int)assetColor)) idMap[(int)assetColor] = new Godot.Collections.Dictionary<int, int>();
            idMap[(int)assetColor][(int)assetPieceType] = id; 
        }

        return (newSprites, idMap);
    }

    private (PlayerColor, PieceType) assetTypeStringToEnums(string assetType) {
        switch(assetType) {
            case "P1_SCOUT":
                return (PlayerColor.PLAYER_1, PieceType.SCOUT);
            case "P1_TILE":
                return (PlayerColor.PLAYER_1, PieceType.TILE);
            case "P2_SCOUT":
                return (PlayerColor.PLAYER_2, PieceType.SCOUT);
            case "P2_TILE":
                return (PlayerColor.PLAYER_2, PieceType.TILE); 
            case "EMPTY_SQUARE":
                return (PlayerColor.BLANK, PieceType.BLANK); 
        }
        GD.Print(assetType + " could not be defined"); 
        return (PlayerColor.MISSING, PieceType.MISSING); 
    }
}
