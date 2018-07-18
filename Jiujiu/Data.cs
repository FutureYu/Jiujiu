using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Jiujiu
{
    static class Data
    {
        public async static Task DeleteAllDataAsync()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Total.dat");
            if (file != null)
            {
                await file.DeleteAsync();
            }
            file = (StorageFile)await folder.TryGetItemAsync("Achi.dat");
            if (file != null)
            {
                await file.DeleteAsync();
            }
            file = (StorageFile)await folder.TryGetItemAsync("Game.dat");
            if (file != null)
            {
                await file.DeleteAsync();
            }
        }
    }

    public class AchievementData
    {
        private bool _a100q;
        private bool _a1000q;
        private bool _a10000q;
        private bool _a30s;
        private bool _a40s;
        private bool _a50s;
        private bool _a7d;
        private bool _a14d;
        private bool _a30d;
        private bool _arate;

        public bool A100q { get => _a100q; set => _a100q = value; }
        public bool A1000q { get => _a1000q; set => _a1000q = value; }
        public bool A10000q { get => _a10000q; set => _a10000q = value; }
        public bool A30s { get => _a30s; set => _a30s = value; }
        public bool A40s { get => _a40s; set => _a40s = value; }
        public bool A50s { get => _a50s; set => _a50s = value; }
        public bool A7d { get => _a7d; set => _a7d = value; }
        public bool A14d { get => _a14d; set => _a14d = value; }
        public bool A30d { get => _a30d; set => _a30d = value; }
        public bool Arate { get => _arate; set => _arate = value; }

        public void SetAchievementData(bool a100q, bool a1000q, bool a10000q, bool a30s, bool a40s, bool a50s, bool a7d, bool a14d, bool a30d, bool arate)
        {
            _a100q = a100q;
            _a1000q = a1000q;
            _a10000q = a10000q;
            _a30s = a30s;
            _a40s = a40s;
            _a50s = a50s;
            _a7d = a7d;
            _a14d = a14d;
            _a30d = a30d;
            _arate = arate;
        }

        public async Task WriteAchievementDataAsync()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Achi.dat");
            if (file == null)
            {
                file = await folder.CreateFileAsync("Achi.dat", CreationCollisionOption.OpenIfExists);
            }
            string content = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", _a100q, _a1000q, _a10000q, _a30s, _a40s, _a50s, _a7d, _a14d, _a30d, _arate);
            await FileIO.WriteTextAsync(file, content);
        }

        public async Task ReadAchievementDataAsync()
        {
            string content;
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Achi.dat");
            if (file != null)
            {
                content = await FileIO.ReadTextAsync(file);
                string[] str = content.Split(',', StringSplitOptions.RemoveEmptyEntries);
                SetAchievementData(Convert.ToBoolean(str[0]), Convert.ToBoolean(str[1]), Convert.ToBoolean(str[2]), Convert.ToBoolean(str[3]), Convert.ToBoolean(str[4]), Convert.ToBoolean(str[5]), Convert.ToBoolean(str[6]), Convert.ToBoolean(str[7]), Convert.ToBoolean(str[8]), Convert.ToBoolean(str[9]));
                return;
            }
            else
            {
                file = await folder.CreateFileAsync("Total.dat", CreationCollisionOption.OpenIfExists);
                SetAchievementData(false, false, false, false, false, false, false, false, false, false);
                await WriteAchievementDataAsync();
            }
            return;
        }

        public async Task DeleteAchievementDataAsync()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Achi.dat");
            if (file != null)
            {
                await file.DeleteAsync();
            }
        }
    }




    class TotalData
    {
        private DateTime _lastLoginDate;
        private DateTime _thisLoginDate;
        private int _totalLoginCount;
        private int _continuousCount;
        private int _totalGameCount;
        private int _totalQuestionCount;
        private int _totalCorrectCount;
        private int _averageScore;
        private int _totalGameTime;

        public DateTime LastLoginDate { get => _lastLoginDate; set => _lastLoginDate = value; }
        public int TotalLoginCount { get => _totalLoginCount; set => _totalLoginCount = value; }
        public int ContinuousCount { get => _continuousCount; set => _continuousCount = value; }
        public int TotalGameCount { get => _totalGameCount; set => _totalGameCount = value; }
        public int TotalQuestionCount { get => _totalQuestionCount; set => _totalQuestionCount = value; }
        public int TotalCorrectCount { get => _totalCorrectCount; set => _totalCorrectCount = value; }
        public int AverageScore { get => _averageScore; set => _averageScore = value; }
        public int TotalGameTime { get => _totalGameTime; set => _totalGameTime = value; }
        public DateTime ThisLoginDate { get => _thisLoginDate; set => _thisLoginDate = value; }

        private void SetTotalData(DateTime lastLoginDate, DateTime thisLoginDate, int totalLoginCount, int continuousCount, int totalGameCount, int totalQuestionCount, int totalCorrectCount, int averageScore, int totalGameTime)
        {
            this._lastLoginDate = lastLoginDate;
            this._thisLoginDate = thisLoginDate;
            this._totalLoginCount = totalLoginCount;
            this._continuousCount = continuousCount;
            this._totalGameCount = totalGameCount;
            this._totalQuestionCount = totalQuestionCount;
            this._totalCorrectCount = totalCorrectCount;
            this._averageScore = averageScore;
            this._totalGameTime = totalGameTime;
        }

        public async Task WriteTotalDataAsync()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Total.dat");
            if (file == null)
            {
                file = await folder.CreateFileAsync("Total.dat", CreationCollisionOption.OpenIfExists);
            }
            string content = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", _lastLoginDate, _thisLoginDate, _totalLoginCount, _continuousCount, _totalGameCount, _totalQuestionCount, _totalCorrectCount, _averageScore, _totalGameTime);
            await FileIO.WriteTextAsync(file, content);
        }

        public async Task ReadTotalDataAsync()
        {
            string content;
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Total.dat");
            if (file != null)
            {
                content = await FileIO.ReadTextAsync(file);
                string[] str = content.Split(',', StringSplitOptions.RemoveEmptyEntries);
                SetTotalData(Convert.ToDateTime(str[0]), Convert.ToDateTime(str[1]), Convert.ToInt32(str[2]), Convert.ToInt32(str[3]), Convert.ToInt32(str[4]), Convert.ToInt32(str[5]), Convert.ToInt32(str[6]), Convert.ToInt32(str[7]), Convert.ToInt32(str[8]));
                return;
            }
            else
            {
                file = await folder.CreateFileAsync("Total.dat", CreationCollisionOption.OpenIfExists);
                SetTotalData(DateTime.Now, DateTime.Now, 1, 1, 0, 0, 0, 0, 0);
                await WriteTotalDataAsync();
            }
            return;
        }

        public async Task DeleteTotalDataAsync()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Total.dat");
            if (file != null)
            {
                await file.DeleteAsync();
            }
        }

    }


    class GameData
    {
        private DateTime _date;
        private int _correctNumber;
        private int _time;
        private int _score;

        public int Score { get => _score; set => _score = value; }
        public int Time { get => _time; set => _time = value; }
        public int CorrectNumber { get => _correctNumber; set => _correctNumber = value; }
        public DateTime Date { get => _date; set => _date = value; }

        public GameData(DateTime date, int correctNumber, int time, int score)
        {
            this._date = date;
            this._correctNumber = correctNumber;
            this._time = time;
            this._score = score;
        }

        public GameData()
        {

        }


        public async Task AppendGameDataAsync()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Game.dat");
            if (file == null)
            {
                file = await folder.CreateFileAsync("Game.dat", CreationCollisionOption.OpenIfExists);
            }
            string content = String.Format("{0},{1},{2},{3}\n", _date, _correctNumber, _time, _score);
            await FileIO.AppendTextAsync(file, content);
        }

        public static async Task<GameData[]> ReadGameDataAsync()
        {
            string content;
            List<GameData> gameDatas = new List<GameData>();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Game.dat");
            if (file != null)
            {
                content = await FileIO.ReadTextAsync(file);
            }
            else
            {
                file = await folder.CreateFileAsync("Game.dat", CreationCollisionOption.OpenIfExists);
                return null;
            }
            string[] strs = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in strs)
            {
                string[] gameStr = str.Split(',', StringSplitOptions.RemoveEmptyEntries);
                gameDatas.Add(new GameData(Convert.ToDateTime(gameStr[0]), Convert.ToInt32(gameStr[1]), Convert.ToInt32(gameStr[2]), Convert.ToInt32(gameStr[3])));
            }
            return gameDatas.ToArray();
        }

        public static async Task DeleteGameDataAsync()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Game.dat");
            if (file != null)
            {
                await file.DeleteAsync();
            }
        }
    }

    class BackgroundData
    {
        public async static Task WriteUriDataAsync(string uri)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Background.dat");
            if (file == null)
            {
                file = await folder.CreateFileAsync("Background.dat", CreationCollisionOption.OpenIfExists);
            }
            await FileIO.WriteTextAsync(file, uri);
        }

        public static async Task<string> ReadUriDataAsync()
        {
            string content;
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Background.dat");
            if (file != null)
            {
                content = await FileIO.ReadTextAsync(file);
            }
            else
            {
                file = await folder.CreateFileAsync("Background.dat", CreationCollisionOption.OpenIfExists);
                return null;
            }
            return content;
        }

        public static async Task DeleteUriDataAsync()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("tmp.png");
            if (file != null)
            {
                await file.DeleteAsync();
            }
        }
    }
}
