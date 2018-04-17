//------------------------------------------------------------------------------
// <copyright file="SmiXetterAccessMap.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <owner current="true" primary="true">Microsoft</owner>
// <owner current="true" primary="false">Microsoft</owner>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.Server {
    using System;
    using System.Diagnostics;

    // Formal encoding of SMI's metadata-to-ITypedSetter/-from-ITypedGetter validity rules
    internal class SmiXetterAccessMap {

        // A couple of private constants to make the getter/setter access tables more readable
        private const bool X = true;
        private const bool _ = false;

        private static bool[,] __isGetterAccessValid = {
        // Getters as columns (abreviated from XetterTypeCode names)
        // SqlDbTypes as rows
        //   bool, byte, bytes, chars, strng, int16, int32, int64, singl, doubl, sqldec, date,  guid, varmd, Xetr, time, dtost
/*BigInt*/  { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  X   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Binary*/  { _  ,  _  ,  X   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Bit*/     { X  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Char*/    { _  ,  _  ,  X   ,  X   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*DTime*/   { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  X  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Decimal*/ { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  X    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Float*/   { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  X   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Image*/   { _  ,  _  ,  X   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Int*/     { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  X   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Money*/   { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  X   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*NChar*/   { _  ,  _  ,  X   ,  X   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*NText*/   { _  ,  _  ,  X   ,  X   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*NVarChar*/{ _  ,  _  ,  X   ,  X   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Real*/    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  X   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*UniqueId*/{ _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   X  ,  _   ,  _  ,  _  ,  _  , },
/*SmDTime*/ { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  X  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*SmInt*/   { _  ,  _  ,  _   ,  _   ,   _  ,   X  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*SmMoney*/ { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  X   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Text*/    { _  ,  _  ,  X   ,  X   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Tstamp*/  { _  ,  _  ,  X   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*TinyInt*/ { _  ,  X  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*VarBin*/  { _  ,  _  ,  X   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*VarChar*/ { _  ,  _  ,  X   ,  X   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Variant*/ { X  ,  X  ,  X   ,  X   ,   X  ,   X  ,  X   ,  X   ,  X   ,  X   ,  X    ,  X  ,   X  ,  X   ,  _  ,  X  ,  X  , },
/* 24 */    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Xml*/     { _  ,  _  ,  X   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/* 26 */    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/* 27 */    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/* 28 */    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Udt*/     { _  ,  _  ,  X   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Struct*/  { _  ,  _  ,  X   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  X  ,  _  ,  _  , },
/*Date*/    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  X  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Time*/    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  X  ,  _  , },
/*DTime2*/  { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  X  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*DTOffset*/{ _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  X  , },
        };

        private static bool[,] __isSetterAccessValid = {
        // Setters as columns (abreviated from XetterTypeCode names)
        // SqlDbTypes as rows
        //      Current difference between setters and getters is that character setters do
        //      not need to support SetBytes
        //   bool, byte, bytes, chars, strng, int16, int32, int64, singl, doubl, sqldec, date,  guid, varmd, Xetr, time, dtost
/*BigInt*/  { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  X   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Binary*/  { _  ,  _  ,  X   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Bit*/     { X  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Char*/    { _  ,  _  ,  _   ,  X   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*DTime*/   { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  X  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Decimal*/ { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  X    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Float*/   { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  X   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Image*/   { _  ,  _  ,  X   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Int*/     { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  X   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Money*/   { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  X   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*NChar*/   { _  ,  _  ,  _   ,  X   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*NText*/   { _  ,  _  ,  _   ,  X   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*NVarChar*/{ _  ,  _  ,  _   ,  X   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Real*/    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  X   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*UniqueId*/{ _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   X  ,  _   ,  _  ,  _  ,  _  , },
/*SmDTime*/ { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  X  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*SmInt*/   { _  ,  _  ,  _   ,  _   ,   _  ,   X  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*SmMoney*/ { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  X   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Text*/    { _  ,  _  ,  _   ,  X   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Tstamp*/  { _  ,  _  ,  X   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*TinyInt*/ { _  ,  X  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*VarBin*/  { _  ,  _  ,  X   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*VarChar*/ { _  ,  _  ,  _   ,  X   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Variant*/ { X  ,  X  ,  X   ,  X   ,   X  ,   X  ,  X   ,  X   ,  X   ,  X   ,  X    ,  X  ,   X  ,  X   ,  _  ,  X  ,  X  , },
/* 24 */    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Xml*/     { _  ,  _  ,  X   ,  _   ,   X  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/* 26 */    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/* 27 */    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/* 28 */    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Udt*/     { _  ,  _  ,  X   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Struct*/  { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  X  ,  _  ,  _  , },
/*Date*/    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  X  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*Time*/    { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  X  ,  _  , },
/*DTime2*/  { _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  X  ,   _  ,  _   ,  _  ,  _  ,  _  , },
/*DTOffset*/{ _  ,  _  ,  _   ,  _   ,   _  ,   _  ,  _   ,  _   ,  _   ,  _   ,  _    ,  _  ,   _  ,  _   ,  _  ,  _  ,  X  , },
        };

        internal static bool IsGetterAccessValid( SmiMetaData metaData, SmiXetterTypeCode xetterType ) {
            // Make sure no-one adds a new xetter type without updating this file!
            Debug.Assert( SmiXetterTypeCode.XetBoolean <= xetterType && SmiXetterTypeCode.XetDateTimeOffset >= xetterType );

            return __isGetterAccessValid[(int) metaData.SqlDbType, (int) xetterType];
        }

        internal static bool IsSetterAccessValid( SmiMetaData metaData, SmiXetterTypeCode xetterType ) {
            // Make sure no-one adds a new xetter type without updating this file!
            Debug.Assert(SmiXetterTypeCode.XetBoolean <= xetterType && SmiXetterTypeCode.XetDateTimeOffset >= xetterType &&
                    SmiXetterTypeCode.GetVariantMetaData != xetterType);

            return __isSetterAccessValid[(int) metaData.SqlDbType, (int) xetterType];
        }
    }
}