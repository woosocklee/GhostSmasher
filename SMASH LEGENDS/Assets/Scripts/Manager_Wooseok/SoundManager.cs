using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Junpyo;
using RotaryHeart.Lib.SerializableDictionary;

namespace Wooseok
{
    
    public class SoundManager : MonoBehaviour
    {


        // �̱��� ������ ����ϱ� ���� �ν��Ͻ� ����
        private static SoundManager _instance;
        // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
        public static SoundManager Instance
        {
            get
            {
                // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        public enum SOUNDTYPE
        {
            BGM = 0,
            SFX = 1,
            EFFECT = 2,
            END = 99
        }

        AudioSource BGMSource;
        AudioSource SFXSource;

        [SerializeField]
        SerializableDictionaryBase<string, AudioSource> effect_audio_sources = new SerializableDictionaryBase<string, AudioSource>();

        public List<AudioClip> BGMaudioclips = new List<AudioClip>();
        public List<AudioClip> SFXaudioclips = new List<AudioClip>();
        public List<AudioClip> EFFECTaudioclips = new List<AudioClip>();


        // 0 �̻� 1 ����. ������.
        public float volumeSFX;
        public float volumeBGM;
        public float volumeEFFECT;

        public void PlayEffectSoundOnce(string target, AudioClip sound)
        {
            if (effect_audio_sources[target].isPlaying)
            {
                effect_audio_sources[target].Stop();
            }
            effect_audio_sources[target].loop = false;
            effect_audio_sources[target].clip = sound;
            effect_audio_sources[target].Play(0);
            Debug.Log(effect_audio_sources[target].isPlaying);
        }

        public void PlayBGMSoundOnce(AudioClip sound)
        {
            if (BGMSource.isPlaying)
            {
                BGMSource.Stop();
            }
            BGMSource.loop = false;
            BGMSource.clip = sound;
            BGMSource.Play();
        }

        public void PlayBGMSoundLoop(AudioClip sound)
        {
            if(!(BGMSource.isPlaying && BGMSource.clip == sound))
            {
                BGMSource.loop = true;
                BGMSource.clip = sound;
                BGMSource.Play();
            }
            else
            {
            }

        }

        public void PlaySFXSoundOnce(AudioClip sound)
        {
            if(SFXSource.isPlaying)
            {
                SFXSource.Stop();
            }
            SFXSource.loop = false;
            SFXSource.clip = sound;
            SFXSource.Play();
        }

        public void RPC()
        {
            // ������̶� ��ũ �����ֱ�
        }

        public void LoadingAudioClips()
        {
            //EFFECTaudioclips.Add(Resources.Load<AudioClip>("Audio_Test/Woo"));
            DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Resources/Audio_Test");
            foreach(var File in directoryInfo.GetFiles())
            {
                Debug.Log(File.Name);
                if(File.Extension == ".mp3")
                {
                    EFFECTaudioclips.Add(Resources.Load("Audio_Test/" + Path.GetFileNameWithoutExtension(File.FullName)) as AudioClip);
                }
            }
            
        }


        void Start()
        {
            AudioSource[] allaudiosources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audiosource in allaudiosources)
            {
                if(audiosource != BGMSource && audiosource != SFXSource)
                {
                    effect_audio_sources.Add(audiosource.gameObject.name,audiosource);
                }
            }

            LoadingAudioClips();

            

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

}