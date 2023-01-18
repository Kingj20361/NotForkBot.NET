﻿
namespace SysBot.Pokemon
{
    /// <summary>
    /// Pokémon Legends: Arceus RAM offsets
    /// </summary>
    public class PokeDataOffsetsSV
    {
        public const string ScarletID = "0100A3D008C5C000";
        public const string VioletID = "01008F6008C5E000";
        public IReadOnlyList<long> BoxStartPokemonPointer { get; } = new long[] { 0x4384B18, 0x128, 0x9B0, 0x0 };
        public IReadOnlyList<long> LinkTradePartnerPokemonPointer { get; } = new long[] { 0x437ECE0, 0x48, 0x58, 0x40, 0x148 };
        public IReadOnlyList<long> LinkTradePartnerNIDPointer { get; } = new long[] { 0x43A28F0, 0xF8, 0x8 };
        public IReadOnlyList<long> MyStatusPointer { get; } = new long[] { 0x4384B18, 0x148, 0x40 };
        public IReadOnlyList<long> Trader1MyStatusPointer { get; } = new long[] { 0x437ECE0, 0x48, 0xB0, 0x0 }; // The trade partner status uses a compact struct that looks like MyStatus.
        public IReadOnlyList<long> Trader2MyStatusPointer { get; } = new long[] { 0x437ECE0, 0x48, 0xE0, 0x0 };
        public IReadOnlyList<long> ConfigPointer { get; } = new long[] { 0x4384B18, 0x1B8, 0x40 };
        public IReadOnlyList<long> CurrentBoxPointer { get; } = new long[] { 0x4384B18, 0x120, 0x570 };
        public IReadOnlyList<long> PortalBoxStatusPointer { get; } = new long[] { 0x439DFF0, 0x18, 0xA0, 0x1B8, 0x70, 0x28 };  // 9-A in portal, 4-6 in box.
        public IReadOnlyList<long> IsConnectedPointer { get; } = new long[] { 0x437E280, 0x30 };
        public IReadOnlyList<long> OverworldPointer { get; } = new long[] { 0x43A7848, 0x348, 0x10, 0xD8, 0x28 };

        public const int BoxFormatSlotSize = 0x158;
        public const ulong LibAppletWeID = 0x010000000000100a; // One of the process IDs for the news.

        public IReadOnlyList<long> TeraRaidCodePointer { get; } = new long[] { 0x437DEC0, 0x98, 0x00, 0x10, 0x30, 0x10, 0x1A9 };
        public IReadOnlyList<long> TeraRaidBlockPointer { get; } = new long[] { 0x4384B18, 0x180, 0x40 };

        public ulong TeraLobby { get; } = 0x0403F4B0;
        public ulong LoadedIntoRaid { get; } = 0x04416020;
    }
}
