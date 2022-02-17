using Beiming.app.login;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;

namespace Beiming
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {

            LogoPanel logo = new LogoPanel();
            logo.Show();

        }

        private void hook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //  这里写具体实现
            if (e.KeyCode.Equals(Keys.Space))
            {
                if (null != k_hook)
                {
                    if (null != waveIn)
                        waveIn.StopRecording();

                    if (null != writer)
                        writer.Dispose();
                    System.Windows.MessageBox.Show("录制完成");
                }
            }
        }

        KeyboardHook k_hook;
        WaveInEvent waveIn;
        private WaveFileWriter writer;


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            KeyEventHandler myKeyEventHandeler = new KeyEventHandler(hook_KeyDown);
            k_hook = new KeyboardHook();
            k_hook.KeyDownEvent += myKeyEventHandeler;//钩住键按下
            k_hook.Start();//安装键盘钩子



            var outputFolder = "D:\\NAudio";// System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "NAudio");
            Directory.CreateDirectory(outputFolder);
            var outputFilePath = System.IO.Path.Combine(outputFolder, "recorded.wav");


            waveIn = new WaveInEvent();
            writer = new WaveFileWriter(outputFilePath, waveIn.WaveFormat); ; //生成音频文件的对象

            //写入文件
            waveIn.DataAvailable += (s, a) =>
            {
                writer.Write(a.Buffer, 0, a.BytesRecorded);

                float[] allSamples = Enumerable      // 提取数据中的采样
                                    .Range(0, a.BytesRecorded / 4)   // 除以四是因为, 缓冲区内每 4 个字节构成一个浮点数, 一个浮点数是一个采样
                                    .Select(i => BitConverter.ToSingle(a.Buffer, i * 4))  // 转换为 float
                                    .ToArray();    // 转换为数组
                                                   // 获取采样后, 在这里进行详细处理

                // 设定我们已经将刚刚的采样保存到了变量 AllSamples 中, 类型为 float[]
                int channelCount = writer.WaveFormat.Channels;   // WasapiLoopbackCapture 的 WaveFormat 指定了当前声音的波形格式, 其中包含就通道数
                float[][] channelSamples = Enumerable
                    .Range(0, channelCount)
                    .Select(channel => Enumerable
                        .Range(0, allSamples.Length / channelCount)
                        .Select(i => allSamples[channel + i * channelCount])
                        .ToArray())
                    .ToArray();


            };

            //结束录音
            waveIn.RecordingStopped += (s, a) =>
            {
                writer.Dispose();
                writer = null;
                waveIn.Dispose();

            };

            try
            {
                waveIn.StartRecording();
            }
            catch (Exception E)
            {
                System.Windows.MessageBox.Show("出现异常" + E);
            }


            // var enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();
            // //允许你在某些状态下枚举渲染设备
            // var endpoints = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Unplugged | DeviceState.Active);
            // foreach (var endpoint in endpoints)
            //
            // {
            //     Console.WriteLine(endpoint.FriendlyName);
            // }
            // // Aswell as hook to the actual event
            //
            // enumerator.RegisterEndpointNotificationCallback(new NotificationClient());
            // Console.ReadLine();
        }
    }




    class NotificationClient : IMMNotificationClient
    {
        void IMMNotificationClient.OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
        {
            throw new NotImplementedException();
        }

        void IMMNotificationClient.OnDeviceAdded(string pwstrDeviceId)
        {
            throw new NotImplementedException();
        }

        void IMMNotificationClient.OnDeviceRemoved(string deviceId)
        {
            throw new NotImplementedException();
        }

        void IMMNotificationClient.OnDeviceStateChanged(string deviceId, DeviceState newState)
        {
            throw new NotImplementedException();
        }

        void IMMNotificationClient.OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
        {
            throw new NotImplementedException();
        }
    }
}
