using HMUI;
using IPA.Utilities;
using Newtonsoft.Json;
using NJOPriset.Models;
using NJOPriset.Patch;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NJOPriset
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class NJOPrisetController : MonoBehaviour
    {
        public ConcurrentDictionary<SongDataEntity, int> NJODatas { get; } = new ConcurrentDictionary<SongDataEntity, int>();
        [Inject]
        public void Constractor(GameplaySetupViewController container, PlayerDataModel model, StandardLevelDetailViewController standard)
        {
            this.gameplaySetupViewController = container;
            this.playerDataModel = model;
            this._standardLevelDetailViewController = standard;
            this._standardLevelDetailViewController.didChangeDifficultyBeatmapEvent += this.Standard_didChangeDifficultyBeatmapEvent;
            this._standardLevelDetailViewController.didPressActionButtonEvent += this._standardLevelDetailViewController_didPressActionButtonEvent;
            this._standardLevelDetailViewController.didChangeContentEvent += this._standardLevelDetailViewController_didChangeContentEvent;
            RefreshContentPatch.OnRefresh += this.RefreshContentPatch_OnRefresh;
            this.playerSettingsPanelController = this.gameplaySetupViewController.GetField<PlayerSettingsPanelController, GameplaySetupViewController>("_playerSettingsPanelController");
            var setting = SettingJson.Load();
            foreach (var item in setting.Songs) {
                this.NJODatas.AddOrUpdate(JsonConvert.DeserializeObject<SongDataEntity>(item.Key), item.Value, (e, v) => item.Value);
            }
        }

        private void _standardLevelDetailViewController_didPressActionButtonEvent(StandardLevelDetailViewController obj)
        {
            this.AddPriset();
        }

        private void RefreshContentPatch_OnRefresh(StandardLevelDetailView obj)
        {
            Plugin.Log.Debug("RefreshContentPatch_OnRefresh call");
            this.SetNJO(obj.selectedDifficultyBeatmap);
        }

        private void _standardLevelDetailViewController_didChangeContentEvent(StandardLevelDetailViewController arg1, StandardLevelDetailViewController.ContentType arg2)
        {
            Plugin.Log.Debug("Standard_didChangeDifficultyBeatmapEvent call");
            if (arg2 == StandardLevelDetailViewController.ContentType.OwnedAndReady) {
                this.SetNJO(arg1.selectedDifficultyBeatmap);
            }
        }

        private void Standard_didChangeDifficultyBeatmapEvent(StandardLevelDetailViewController arg1, IDifficultyBeatmap arg2)
        {
            Plugin.Log.Debug("Standard_didChangeDifficultyBeatmapEvent call");
            this.SetNJO(arg2);
        }

        private void SetNJO(IDifficultyBeatmap arg2)
        {
            try {
                var parentCharaName = arg2.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName;
                var diff = (int)arg2.difficulty;
                var levelID = arg2.level.levelID;
                if (this.NJODatas.TryGetValue(new SongDataEntity(levelID, parentCharaName, diff), out var njo)) {
                    this._simpleTextDropdown.SelectCellWithIdx(njo);
                    var table = this._simpleTextDropdown.GetField<TableView, DropdownWithTableView>("_tableView");
                    this._simpleTextDropdown.HandleTableViewDidSelectCellWithIdx(table, njo);
                }
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
            }
        }


        private void Start()
        {
            this.noteJumpStartBeatOffsetDropdown = this.playerSettingsPanelController.GetField<NoteJumpStartBeatOffsetDropdown, PlayerSettingsPanelController>("_noteJumpStartBeatOffsetDropdown");
            this._simpleTextDropdown = this.noteJumpStartBeatOffsetDropdown.GetField<SimpleTextDropdown, NoteJumpStartBeatOffsetDropdown>("_simpleTextDropdown");
        }

        private void OnDestroy()
        {
            this._standardLevelDetailViewController.didChangeDifficultyBeatmapEvent -= this.Standard_didChangeDifficultyBeatmapEvent;
            this._standardLevelDetailViewController.didPressActionButtonEvent -= this._standardLevelDetailViewController_didPressActionButtonEvent;
            this._standardLevelDetailViewController.didChangeContentEvent -= this._standardLevelDetailViewController_didChangeContentEvent;
            RefreshContentPatch.OnRefresh -= this.RefreshContentPatch_OnRefresh;
        }

        public void AddPriset()
        {
            Plugin.Log.Debug("AddPriset call");
            try {
                var beatmap = this._standardLevelDetailViewController.selectedDifficultyBeatmap;
                var parentCharaName = beatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName;
                var diff = (int)beatmap.difficulty;
                var levelID = beatmap.level.levelID;
                var njo = this.noteJumpStartBeatOffsetDropdown.GetIdxForOffset(this.playerDataModel.playerData.playerSpecificSettings.noteJumpStartBeatOffset);
                this.NJODatas.AddOrUpdate(new SongDataEntity(levelID, parentCharaName, diff), njo, (e, v) => njo);
                var setting = SettingJson.Load();
                foreach (var item in this.NJODatas) {
                    setting.Songs.AddOrUpdate(JsonConvert.SerializeObject(item.Key), item.Value, (k, v) => item.Value);
                }
                setting.Save();
                Plugin.Log.Debug($"Offset ID:{njo}");
                Plugin.Log.Debug($"Offset value:{this.playerDataModel.playerData.playerSpecificSettings.noteJumpStartBeatOffset}");
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
            }
        }
        //private Button _actionButton;
        private PlayerDataModel playerDataModel;
        private GameplaySetupViewController gameplaySetupViewController;
        private PlayerSettingsPanelController playerSettingsPanelController;
        private NoteJumpStartBeatOffsetDropdown noteJumpStartBeatOffsetDropdown;
        private StandardLevelDetailViewController _standardLevelDetailViewController;
        private SimpleTextDropdown _simpleTextDropdown;
    }
}
