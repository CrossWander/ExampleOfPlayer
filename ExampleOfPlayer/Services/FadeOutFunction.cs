using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExampleOfPlayer.Audio_lib;
using System.Windows.Forms;
using System.Threading;

namespace ExampleOfPlayer.Services
{
    class FadeOutFunction
    {
        // затухание по окончанию трека...за несколько секунд до конца включается затухание
        // затухать пока Volume > 1....можно по событию таймера делать - сравнивая заранее вычесленное время трека минус
        // время на затухание и то сколько времени прошло после воспроизведения, после запускать функцию, 
        // которая постепенно уменьшает громкость....Можно также сделать и для запуска песни из затухания пойдет песня

        //вызов затухания - это как нажатие клавиши стоп...



        //Сделать проверку, если уменьшается звук, а нажата кнопка плей или след трек, то выполнять увеличение звука
        //в новом потоке, чтобы не мешать затуханию
        //если была нажата конпка паузы - продолжить увеличение(затухание) с момента паузы
		
		private static int INT_VOLUME_MAX = 100;
		private static int VOLUME_MIN = 0;
		private static float FLOAT_VOLUME_MAX = 1;
		private static float FLOAT_VOLUME_MIN = 0;
		

        public async Task FadeOut(float duration, BasicPlayerFunction _basicplayer )
        {
            try
            {
                float time = duration;
                while (_basicplayer.Volume > VOLUME_MIN)
                {
                    time -= 20;
                    float volume = ((float)_basicplayer.Volume * time) / duration;
                    await Task.Delay(100);
                    await Task.Run(() =>
                    {
                        _basicplayer.Volume = volume;
                    });
                } 
                _basicplayer.Stop();
            }
            catch (OperationCanceledException ex)
            {
                 //Do stuff to handle cancellation 
                 MessageBox.Show("OperationCanceledException "+ ex);
            }
        }

/// <summary>
/// не работает
/// </summary>
/// <param name="delay"></param>
/// <param name="_basicplayer"></param>
/// <returns></returns>
        public async Task FadeOut2(float delay, BasicPlayerFunction _basicplayer)
        {
            try
            {
                TimeSpan time = TimeSpan.FromMilliseconds(delay);
                int iVolume = (int)_basicplayer.Volume;//*100;
                while (_basicplayer.Volume > VOLUME_MIN)
                {
                    iVolume = iVolume - 2;
                    double fVolume = 1 - ((float)Math.Log(INT_VOLUME_MAX - iVolume) / (float)Math.Log(INT_VOLUME_MAX));
                    if (fVolume < FLOAT_VOLUME_MIN)
                    {
                        fVolume = FLOAT_VOLUME_MIN;
                        break;
                    }                 
                    await Task.Delay(time);
                    await Task.Run(() =>
                    {
                        _basicplayer.Volume = fVolume;
                    });
                }
                _basicplayer.Stop();
            }
            catch (OperationCanceledException ex)
            {
                //Do stuff to handle cancellation 
                MessageBox.Show("OperationCanceledException " + ex);
            }
        }


        public async Task FadeIn(float delay, BasicPlayerFunction _basicplayer)
        {
            try
            {
                TimeSpan time = TimeSpan.FromMilliseconds(delay);
                int iVolume = 0;//(int)_basicplayer.Volume;
                _basicplayer.Play();
                while (_basicplayer.Volume < FLOAT_VOLUME_MAX)
                {
                    iVolume = iVolume + 4;
                    if (iVolume > INT_VOLUME_MAX)
                        iVolume = INT_VOLUME_MAX;
                    float fVolume = 1 - ((float)Math.Log(INT_VOLUME_MAX - iVolume) / (float)Math.Log(INT_VOLUME_MAX));
                    if (fVolume > FLOAT_VOLUME_MAX)
                    {
                       // _basicplayer.Volume = INT_VOLUME_MAX;
                        fVolume = FLOAT_VOLUME_MAX;
                      //  MessageBox.Show("Doshol do 100");
                      //  break;
                    }

                    await Task.Delay(time);
                    await Task.Run(() =>
                    {
                        if (Properties.Settings.Default.AudioLibrary == (int)MediaEngine.Wmp)
                            fVolume *= 100;
                        _basicplayer.Volume = fVolume;
                    });
                }
                return;               
            }
            catch (OperationCanceledException ex)
            {
                //Do stuff to handle cancellation 
                MessageBox.Show("OperationCanceledException " + ex);
            }
        }
/// <summary>
/// не работает
/// </summary>
/// <param name="duration"></param>
/// <param name="_basicplayer"></param>
/// <returns></returns>
        public async Task FadeIn2(float duration, BasicPlayerFunction _basicplayer)
        {
            try
            {
                float time = 0;
                _basicplayer.Play();
                _basicplayer.Volume = 100 / INT_VOLUME_MAX;
                while (_basicplayer.Volume < INT_VOLUME_MAX)
                {
                    time += 100;
                    float volume = ((float)_basicplayer.Volume * time) / duration;
                    await Task.Delay(100);
                    await Task.Run(() =>
                    {
                        _basicplayer.Volume = volume;
                    });
                }
            }
            catch (OperationCanceledException ex)
            {
                //Do stuff to handle cancellation 
                MessageBox.Show("OperationCanceledException " + ex);
            }
        }

    }
}
