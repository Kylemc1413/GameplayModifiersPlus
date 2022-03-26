using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPA.Utilities;
namespace GamePlayModifiersPlus.Utilities
{
    public static class BeatmapHelperExtensions
    {
        internal static readonly FieldAccessor<BeatmapCallbacksController, IReadonlyBeatmapData>.Accessor _beatmapDataAccessor = FieldAccessor<BeatmapCallbacksController, IReadonlyBeatmapData>.GetAccessor("_beatmapData");
        public static void Update(this BeatmapObjectSpawnController.InitData data, float njs, float noteJumpOffset, float? bpm = null)
        {
            data.SetField("noteJumpMovementSpeed", njs);
            data.SetField("noteJumpValue", noteJumpOffset);
            if (bpm != null)
                data.SetField("beatsPerMinute", bpm.Value);
        }
        public static BeatmapData GetDataCopy(this BeatmapCallbacksController callbackController)
        {
            var beatmapData = _beatmapDataAccessor(ref callbackController) as BeatmapData;
            return beatmapData.GetCopy();
        }
        public static void ReplaceData(this BeatmapCallbacksController callbackController, BeatmapData newData)
        {
            _beatmapDataAccessor(ref callbackController) = newData;
        }
        public static void AddObjectsToBeatmap(this BeatmapCallbacksController callbackController, List<BeatmapObjectData> items)
        {
            var beatmapData = _beatmapDataAccessor(ref callbackController) as BeatmapData;
            foreach (var item in items)
                beatmapData.AddBeatmapObjectData(item);
        }
        public static void AddEventsToBeatmap(this BeatmapCallbacksController callbackController, List<BeatmapEventData> items)
        {
            var beatmapData = _beatmapDataAccessor(ref callbackController) as BeatmapData;
            foreach (var item in items)
                beatmapData.InsertBeatmapEventData(item);
        }

        public static void ModifyBeatmap(this BeatmapCallbacksController callbackController, Func<BeatmapDataItem, BeatmapDataItem> func, float startTime = 0, float endTime = float.MaxValue)
        {
            var beatmapData = _beatmapDataAccessor(ref callbackController) as BeatmapData;
            _beatmapDataAccessor(ref callbackController) = beatmapData.GetFilteredCopy(x => {

                if (x.time > startTime && x.time < endTime)
                    return func(x);
                return x;
            });
            callbackController.ResetCallbacksController();
        }
       
        public static LinkedListNode<BeatmapDataItem> GetLastNode(this BeatmapCallbacksController callbackController, float aheadTime)
        {
            var dic = callbackController.GetField<Dictionary<float, CallbacksInTime>, BeatmapCallbacksController>("_callbacksInTimes");
            if(dic.TryGetValue(aheadTime, out var callback))
            {
                return callback.lastProcessedNode;
            }
            return null;
        }

        public static void SetNewLastNodeForCallback(this BeatmapCallbacksController callbackController, LinkedListNode<BeatmapDataItem> item, float aheadTime)
        {
            var dic = callbackController.GetField<Dictionary<float, CallbacksInTime>, BeatmapCallbacksController>("_callbacksInTimes");
            if (dic.TryGetValue(aheadTime, out var callback))
            {
                callback.lastProcessedNode = item;
            }
        }

        public static void ResetCallbacksController(this BeatmapCallbacksController callbackController, float? prevSongTime = null, float? startFilterTime = null)
        {
            if(prevSongTime != null)
                callbackController.SetField("_prevSongTime", prevSongTime.Value);
            if(startFilterTime != null)
                callbackController.SetField("_startFilterTime", startFilterTime.Value);
            var dic = callbackController.GetField<Dictionary<float, CallbacksInTime>, BeatmapCallbacksController>("_callbacksInTimes");
            foreach (var item in dic.Values)
            {
                item.lastProcessedNode = null;
            }
        }
    }
}
