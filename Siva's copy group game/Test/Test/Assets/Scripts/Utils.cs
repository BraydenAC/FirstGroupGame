using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static string GetKeyName(this LocalizedString localizedString)
        {
            var tableEntry = LocalizationSettings.StringDatabase.GetTableEntry(localizedString.TableReference, localizedString.TableEntryReference);
            return tableEntry.Entry.SharedEntry.Key;
        }

        public static string GetKeyName<T>(this LocalizedAsset<T> localizedAsset) where T : UnityEngine.Object
        {
            return localizedAsset.TableEntryReference.Key;
        }
        public static string GetTableName(this LocalizedString localizedString) => localizedString.TableReference.TableCollectionName;
        public static string GetTableName<T>(this LocalizedAsset<T> localizedAsset) where T: UnityEngine.Object => localizedAsset.TableReference.TableCollectionName;

        public static void Enumerate<T>(this IEnumerable<T> enumerable, Action<int, T> action)
        {
            int i = 0;
            foreach (T item in enumerable)
            {
                action(i, item);
                i++;
            }
        }
    }
}