using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Athena.Core.Internal.GameManager.DBC.Internal;
using Athena.Core.Internal.GameManager.DBC.Internal.Rows;
using Athena.Core.Patchables;

namespace Athena.Core.Internal.GameManager.DBC
{
    public class DBCManager
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate uint DBCGetCompressedRows(uint dbc, uint allocatedSapce);
        public static DBCGetCompressedRows _GetCompressedRows;

        #region LoadedDBCS
        public static Dictionary<int, SpellDBC> SpellDBCs = new Dictionary<int, SpellDBC>();
        public static Dictionary<int, ItemSubClassDBC> ItemSubClassDBCs = new Dictionary<int, ItemSubClassDBC>();
        public static Dictionary<int, ResearchSiteDBC> ResearchSiteDBCs = new Dictionary<int, ResearchSiteDBC>();
        public static Dictionary<int, SpellRangeDBC> SpellRangeDBCs = new Dictionary<int, SpellRangeDBC>();
        public static Dictionary<int, SpellCategoriesDBC> SpellCategoriesDBCs = new Dictionary<int, SpellCategoriesDBC>();
        public static Dictionary<int, LockDBC> LockDBCs = new Dictionary<int, LockDBC>();
        public static Dictionary<int, SpellCastTimesDBC> SpellCastTimesDBCs = new Dictionary<int, SpellCastTimesDBC>();
        public static Dictionary<int, SpellCooldownsDBC> SpellCooldownsDBCs = new Dictionary<int, SpellCooldownsDBC>();
        public static Dictionary<int, SpellEffectDBC> SpellEffectDBCs = new Dictionary<int, SpellEffectDBC>();
        public static Dictionary<int, QuestPOIPointDBC> QuestPOIDBCs = new Dictionary<int, QuestPOIPointDBC>();
        #endregion


        #region Initialize Functions
        public void Initialize()
        {
            _GetCompressedRows =
                GeneralHelper.Memory.CreateFunction<DBCGetCompressedRows>(
                    Offsets.UncataloguedFunctions.DBCGetCompressedRows);

            InitializeDBC();
            //InitializeDBCCompressed();
            //InitializeSpellEffects();
        }

        private static void InitializeDBC()
        {
            DBCManager.ResearchSiteDBCs = DBCManager.ReadDBCRows<ResearchSiteDBC>(Offsets.ClientDbOffsets.ResearchSite);
            GeneralHelper.MainLog("ResearchSite.dbc loaded, " + (object)DBCManager.ResearchSiteDBCs.Count + " rows", "Debug");

            DBCManager.ItemSubClassDBCs = DBCManager.ReadDBCRows<ItemSubClassDBC>(Offsets.ClientDbOffsets.ItemSubClass);
            GeneralHelper.MainLog("ItemSubClass.dbc loaded, " + (object)DBCManager.ItemSubClassDBCs.Count + " rows", "Debug");

            DBCManager.SpellRangeDBCs = DBCManager.ReadDBCRows<SpellRangeDBC>(Offsets.ClientDbOffsets.SpellRange);
            GeneralHelper.MainLog("SpellRange.dbc loaded, " + (object)DBCManager.SpellRangeDBCs.Count + " rows", "Debug");

            DBCManager.SpellCategoriesDBCs = DBCManager.ReadDBCRows<SpellCategoriesDBC>(Offsets.ClientDbOffsets.SpellCategories);
            GeneralHelper.MainLog("SpellCategories.dbc loaded, " + (object)DBCManager.SpellCategoriesDBCs.Count + " rows", "Debug");

            DBCManager.LockDBCs = DBCManager.ReadDBCRows<LockDBC>(Offsets.ClientDbOffsets.Lock);
            GeneralHelper.MainLog("Lock.dbc loaded, " + (object)DBCManager.LockDBCs.Count + " rows", "Debug");

            DBCManager.SpellCastTimesDBCs = DBCManager.ReadDBCRows<SpellCastTimesDBC>(Offsets.ClientDbOffsets.SpellCastTimes);
            GeneralHelper.MainLog("SpellCastTimes.dbc loaded, " + (object)DBCManager.SpellCastTimesDBCs.Count + " rows", "Debug");

            DBCManager.SpellCooldownsDBCs = DBCManager.ReadDBCRows<SpellCooldownsDBC>(Offsets.ClientDbOffsets.SpellCooldowns);
            GeneralHelper.MainLog("SpellCooldowns.dbc loaded, " + (object)DBCManager.SpellCooldownsDBCs.Count + " rows", "Debug");

            DBCManager.QuestPOIDBCs = DBCManager.ReadDBCRows<QuestPOIPointDBC>(Offsets.ClientDbOffsets.QuestPOIPoint);
            GeneralHelper.MainLog("QuestDBC.dbc loaded, " + (object)DBCManager.QuestPOIDBCs.Count + " rows", "Debug");
        }

        private static void InitializeDBCCompressed()
        {
            //DBCManager.SpellDBCs = DBCManager.ReadDBCCompressedRows<SpellDBC>(Offsets.ClientDbOffsets.Spell);
            //GeneralHelper.MainLog("Spell.dbc loaded, " + (object)DBCManager.SpellDBCs.Count + " rows", "Debug");

//            DBCManager.SpellEffectDBCs = DBCManager.ReadDBCCompressedRows<SpellEffectDBC>(Offsets.ClientDbOffsets.SpellEffect);
  //          GeneralHelper.MainLog("SpellEffect.dbc loaded, " + (object)DBCManager.SpellEffectDBCs.Count + " rows", "Debug");
        }
        #endregion



        #region Get Rows Functions
        public static Dictionary<int, T> ReadDBCRows<T>(uint DBC) where T : DBCRow, new()
        {
            Dictionary<int, T> dictionary = new Dictionary<int, T>();

            T tempInstance = new T();

            foreach (KeyValuePair<uint, uint> keyValuePair in GetRowEntriesAndPointers(DBC, tempInstance.RowSize))
            {
                if ((int)keyValuePair.Value != 0)
                {
                    T instance = Activator.CreateInstance<T>();
                    instance.Pointer = keyValuePair.Value;
                    instance.Initialize(keyValuePair.Value);
                    dictionary.Add((int)keyValuePair.Key, instance);
                }
            }
            return dictionary;
        }

        private static Dictionary<int, T> ReadDBCCompressedRows<T>(uint DBC) where T : DBCRow, new()
        {
            Dictionary<int, T> dictionary = new Dictionary<int, T>();

            uint mem = (uint) Marshal.AllocHGlobal(100000000);
            Internal.DBC tempDbc = new Internal.DBC((IntPtr) DBC, 0 /*we dont need a row size for this one*/);

            uint MinIndex = (uint) tempDbc.MinIndex;
            uint MaxIndex = (uint) tempDbc.MaxIndex;

            uint RowPtr = 0;
            //uint currentRow = 0;
            for (uint a = MinIndex; a < MaxIndex; a++)
            {
                RowPtr = _GetCompressedRows(DBC, mem);

                if (RowPtr != 0)
                {
                    break;
                   // currentRow = a;
                }
            }

            T firstInstance = Activator.CreateInstance<T>();
            firstInstance.Pointer = RowPtr;
            firstInstance.Initialize(RowPtr);
            dictionary.Add((int)firstInstance.Entry, firstInstance);
            RowPtr = RowPtr + firstInstance.RowSize;

            for (uint i = 1; i < tempDbc.NumRows; i++)
            {
                T tempInstance = Activator.CreateInstance<T>();
                tempInstance.Pointer = RowPtr;
                tempInstance.Initialize(RowPtr);
                dictionary.Add((int)tempInstance.Entry, tempInstance);
                RowPtr = RowPtr + firstInstance.RowSize;
            }

            //for (uint i = MinIndex; i < MaxIndex; i++)
            //{
            //    uint rowPtr = _GetCompressedRows(DBC, i);
            //    while (rowPtr == 0)
            //    if (rowPtr != 0)
            //    {
            //        T instance = Activator.CreateInstance<T>();
            //        instance.Pointer = rowPtr;
            //        instance.Initialize(rowPtr);
            //        //I need to be able to do like

            //        //uint entry = 0;
            //        //if (typeof (T) == typeof (SpellDBC))
            //        //{
            //        //    SpellDBC o = instance as SpellDBC;
            //        //    entry = o.SpellId;
            //        //}
            //        //else if (typeof (T) == typeof (SpellEffectDBC))
            //        //{
            //        //    SpellEffectDBC o = instance as SpellEffectDBC;
            //        //    entry = o.Id;
            //        //}

            //        dictionary.Add((int) instance.Entry, instance);
            //    }
            //}

            return dictionary;
        }
        

        public static Dictionary<uint, uint> GetRowEntriesAndPointers(uint pointer, uint rowSize)
        {
            Dictionary<uint, uint> dictionary = new Dictionary<uint, uint>();

            Internal.DBC tempDbc = new Internal.DBC((IntPtr)pointer, rowSize);

            for (int i = 0; i < tempDbc.NumRows; i++)
            {
                uint rowEntry = tempDbc.GetRowEntry(i);
                uint rowPointer = tempDbc.GetRowPtr(i);

                dictionary.Add(rowEntry, rowPointer);
            }

            return dictionary;
        }
        #endregion
    }
}
