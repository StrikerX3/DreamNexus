﻿using System;
using System.Collections.Generic;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IMoveCollection
    {
        IDictionary<WazaIndex, WazaDataInfo.Entry> LoadedMoves { get; }
        void SetMove(WazaIndex id, WazaDataInfo.Entry model);
        bool IsMoveDirty(WazaIndex id);
        WazaDataInfo.Entry? GetMoveById(WazaIndex id);
        void Flush(IRtdxRom rom);
    }

    public class MoveCollection : IMoveCollection
    {
        public IDictionary<WazaIndex, WazaDataInfo.Entry> LoadedMoves { get; } = new Dictionary<WazaIndex, WazaDataInfo.Entry>();
        public HashSet<WazaIndex> DirtyMoves { get; } = new HashSet<WazaIndex>();

        private IRtdxRom rom;

        public MoveCollection(IRtdxRom rom)
        {
            this.rom = rom;
        }

        public WazaDataInfo.Entry LoadMove(WazaIndex index)
        {
            var data = rom.GetWazaDataInfo().Entries[(int) index];
            return data.Clone();
        }

        public WazaDataInfo.Entry GetMoveById(WazaIndex id)
        {
            DirtyMoves.Add(id);
            if (!LoadedMoves.ContainsKey(id))
            {
                LoadedMoves.Add(id, LoadMove(id));
            }
            return LoadedMoves[id];
        }

        public void SetMove(WazaIndex id, WazaDataInfo.Entry model)
        {
            LoadedMoves[id] = model;
        }

        public bool IsMoveDirty(WazaIndex id)
        {
            return DirtyMoves.Contains(id);
        }

        public void Flush(IRtdxRom rom)
        {
            var romMoves = rom.GetWazaDataInfo().Entries;
            foreach (var move in LoadedMoves)
            {
                romMoves[(int) move.Key] = move.Value;
            }
        }
    }    
}
