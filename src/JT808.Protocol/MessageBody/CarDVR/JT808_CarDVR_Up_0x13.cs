﻿using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.Formatters;
using JT808.Protocol.Interfaces;
using JT808.Protocol.MessagePack;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JT808.Protocol.MessageBody.CarDVR
{
    /// <summary>
    /// 采集指定的外部供电记录
    /// 返回：符合条件的供电记录
    /// </summary>
    public class JT808_CarDVR_Up_0x13 : JT808CarDVRUpBodies, IJT808Analyze
    {
        public override byte CommandId =>  JT808CarDVRCommandID.采集指定的外部供电记录.ToByteValue();
        /// <summary>
        /// 请求发送指定的时间范围内 N 个单位数据块的数据（N≥1）
        /// </summary>
        public List<JT808_CarDVR_Up_0x13_ExternalPowerSupply> JT808_CarDVR_Up_0x13_ExternalPowerSupplys { get; set; }
        public override string Description => "符合条件的供电记录";

        public void Analyze(ref JT808MessagePackReader reader, Utf8JsonWriter writer, IJT808Config config)
        {

        }

        public override JT808CarDVRUpBodies Deserialize(ref JT808MessagePackReader reader, IJT808Config config)
        {
            JT808_CarDVR_Up_0x13 value = new JT808_CarDVR_Up_0x13();
            value.JT808_CarDVR_Up_0x13_ExternalPowerSupplys = new List<JT808_CarDVR_Up_0x13_ExternalPowerSupply>();
            var count = (reader.ReadCurrentRemainContentLength() - 1 - 1) / 7;//记录块个数, -1 去掉808校验位，-1去掉808尾部标志
            for (int i = 0; i < count; i++)
            {
                JT808_CarDVR_Up_0x13_ExternalPowerSupply jT808_CarDVR_Up_0x13_ExternalPowerSupply = new JT808_CarDVR_Up_0x13_ExternalPowerSupply();
                jT808_CarDVR_Up_0x13_ExternalPowerSupply.EventTime = reader.ReadDateTime6();
                jT808_CarDVR_Up_0x13_ExternalPowerSupply.EventType = reader.ReadByte();
                value.JT808_CarDVR_Up_0x13_ExternalPowerSupplys.Add(jT808_CarDVR_Up_0x13_ExternalPowerSupply);
            }
            return value;
        }

        public override void Serialize(ref JT808MessagePackWriter writer, JT808CarDVRUpBodies jT808CarDVRUpBodies, IJT808Config config)
        {
            JT808_CarDVR_Up_0x13 value = jT808CarDVRUpBodies as JT808_CarDVR_Up_0x13;
            foreach (var externalPowerSupply in value.JT808_CarDVR_Up_0x13_ExternalPowerSupplys)
            {
                writer.WriteDateTime6(externalPowerSupply.EventTime);
                writer.WriteByte(externalPowerSupply.EventType);
            }
        }
        /// <summary>
        /// 单位记录仪外部供电记录数据块格式
        /// </summary>
        public class JT808_CarDVR_Up_0x13_ExternalPowerSupply
        {
            /// <summary>
            ///  事件发生时间
            /// </summary>
            public DateTime EventTime { get; set; }
            /// <summary>
            /// 事件类型
            /// </summary>
            public byte EventType { get; set; }
        }
    }
}
