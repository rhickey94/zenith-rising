using Godot;
using System;
using System.Text.Json;

namespace ZenithRising.Scripts.Core;

public partial class SaveManager : Node
{
    // ===== CONSTANTS =====
    private const string SaveFilePath = "user://save_game.json";
    private const int CurrentSaveVersion = 1;

    // ===== SINGLETON =====
    public static SaveManager Instance { get; private set; }

    // ===== LIFECYCLE =====
    public override void _Ready()
    {
        Instance = this;
    }

    // ===== PUBLIC API =====
    public void SaveGame(SaveData data)
    {
        try
        {
            // Serialize SaveData to JSON
            string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true // Pretty-print for debugging
            });

            // Write to user://save_game.json
            using var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Write);
            if (file == null)
            {
                GD.PrintErr($"Failed to open save file for writing: {FileAccess.GetOpenError()}");
                return;
            }

            file.StoreString(jsonString);
            GD.Print($"Game saved successfully to {SaveFilePath}");
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error saving game: {ex.Message}");
        }
    }


    public SaveData? LoadGame()
    {
        try
        {
            // Check if file exists
            if (!FileAccess.FileExists(SaveFilePath))
            {
                GD.Print("No save file found");
                return null;
            }

            // Read JSON
            using var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Read);
            if (file == null)
            {
                GD.PrintErr($"Failed to open save file for reading: {FileAccess.GetOpenError()}");
                return null;
            }

            string jsonString = file.GetAsText();

            // Deserialize to SaveData
            SaveData data = JsonSerializer.Deserialize<SaveData>(jsonString);

            // Validate version
            if (data.Version != CurrentSaveVersion)
            {
                GD.PrintErr($"Save version mismatch: expected {CurrentSaveVersion}, got {data.Version}");
                return null;
            }

            GD.Print($"Game loaded successfully from {SaveFilePath}");
            return data;
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error loading game: {ex.Message}");
            return null;
        }
    }


    public void DeleteSave()
    {
        try
        {
            if (FileAccess.FileExists(SaveFilePath))
            {
                DirAccess.RemoveAbsolute(SaveFilePath);
                GD.Print($"Save file deleted: {SaveFilePath}");
            }
            else
            {
                GD.Print("No save file to delete");
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error deleting save: {ex.Message}");
        }
    }

    public bool SaveExists()
    {
        return FileAccess.FileExists(SaveFilePath);
    }
}

