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
    /// 采集指定的驾驶人身份记录
    /// 返回：符合条件的驾驶人登录退出记录
    /// </summary>
    public class JT808_CarDVR_Up_0x12 : JT808CarDVRUpBodies, IJT808Analyze
    {
        public override byte CommandId => JT808CarDVRCommandID.采集指定的驾驶人身份记录.ToByteValue();
        /// <summary>
        /// 请求发送指定的时间范围内 N 个单位数据块的数据（N≥1）
        /// </summary>
        public List<JT808_CarDVR_Up_0x12_DriveLogin> JT808_CarDVR_Up_0x12_DriveLogins { get; set; }
        public override string Description => "符合条件的驾驶人登录退出记录";

        public void Analyze(ref JT808MessagePackReader reader, Utf8JsonWriter writer, IJT808Config config)
        {

        }

        public override JT808CarDVRUpBodies Deserialize(ref JT808MessagePackReader reader, IJT808Config config)
        {
            JT808_CarDVR_Up_0x12 value = new JT808_CarDVR_Up_0x12();
            value.JT808_CarDVR_Up_0x12_DriveLogins= new List<JT808_CarDVR_Up_0x12_DriveLogin>();
            var count = (reader.ReadCurrentRemainContentLength() - 1 - 1) / 25;//记录块个数, -1 去掉808校验位，-1去掉808尾部标志
            for (int i = 0; i < count; i++)
            {
                JT808_CarDVR_Up_0x12_DriveLogin jT808_CarDVR_Up_0x12_DriveLogin = new JT808_CarDVR_Up_0x12_DriveLogin();
                jT808_CarDVR_Up_0x12_DriveLogin.LoginTime = reader.ReadDateTime6();
                jT808_CarDVR_Up_0x12_DriveLogin.DriverLicenseNo = reader.ReadASCII(18);
                jT808_CarDVR_Up_0x12_DriveLogin.LoginType = reader.ReadByte();
                value.JT808_CarDVR_Up_0x12_DriveLogins.Add(jT808_CarDVR_Up_0x12_DriveLogin);
            }
            return value;
        }

        public override void Serialize(ref JT808MessagePackWriter writer, JT808CarDVRUpBodies jT808CarDVRUpBodies, IJT808Config config)
        {
            JT808_CarDVR_Up_0x12 value = jT808CarDVRUpBodies as JT808_CarDVR_Up_0x12;
            foreach (var driveLogin in value.JT808_CarDVR_Up_0x12_DriveLogins)
            {
                writer.WriteDateTime6(driveLogin.LoginTime);
                writer.WriteASCII(driveLogin.DriverLicenseNo.PadRight(18, '0'));
                writer.WriteByte(driveLogin.LoginType);
            }
        }
        /// <summary>
        /// 单位驾驶人身份记录数据块格式
        /// </summary>
        public class JT808_CarDVR_Up_0x12_DriveLogin
        {
            /// <summary>
            /// 登入登出时间发生时间
            /// </summary>
            public DateTime LoginTime { get; set; }
            /// <summary>
            /// 机动车驾驶证号码 18位
            /// </summary>
            public string DriverLicenseNo { get; set; }
            /// <summary>
            /// 事件类型
            /// </summary>
            public byte LoginType { get; set; }
        }
    }
}
