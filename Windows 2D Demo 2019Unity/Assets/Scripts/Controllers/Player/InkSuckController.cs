using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkSuckController : MonoBehaviour
{
    public 时间控制器 timeController;
    public float TimePerAdd;
    public List<AudioClip> clips;
    private AudioSource suckSFX;
    private float TempTimer = Mathf.Infinity;

    public 玩家纹理动画器 玩家动画;

    public bool 吸墨中;

    public ParticleSystem InkParticleSystem;
    void Start()
    {
        suckSFX = this.gameObject.AddComponent<AudioSource>();
        suckSFX.playOnAwake = false;
        suckSFX.loop = false;
        suckSFX.volume = 0.4f;
        suckSFX.clip = clips[Random.Range(0, clips.Count)];

        suckSFX.time = 0;

        InkParticleSystem.Stop();
    }
    void Update()
    {
        if (suckSFX.time == suckSFX.clip.length || timeController.GameEnded())
        {
            suckSFX.clip = clips[Random.Range(0, clips.Count)];
            suckSFX.time = 0;
            suckSFX.Stop();
        }
        if(2 <= timeController.getLastShowTimeAdd() && !suckSFX.isPlaying)
        {
            suckSFX.Play();
        }
        if(timeController.剩余时间 < 0)
        {
            suckSFX.Stop();
            吸墨中 = false;
        }
        if (!suckSFX.isPlaying || timeController.getLastShowTimeAdd() < timeController.TipDisplayTime * 0.95f)
        {
            吸墨中 = false;
        }
        TempTimer = timeController.剩余时间;
    }

    public void OnTriggerEnter(Collider col)
    {
        OnTriggerStay(col);
    }

    public void OnTriggerStay(Collider col)
    {
        InkHandler handler = col.gameObject.GetComponentInParent<InkHandler>();
        if(handler != null)
        {
            Suck(col);
            return;
        }
        handler = col.gameObject.GetComponent<InkHandler>();
        if (handler != null)
        {
            Suck(col);
            return;
        }
        handler = col.gameObject.GetComponentInChildren<InkHandler>();
        if (handler != null)
        {
            Suck(col);
            return;
        }

        InversedInkHandler InvHandler = col.gameObject.GetComponentInParent<InversedInkHandler>();
        if (InvHandler != null)
        {
            Give(col);
            return;
        }
        InvHandler = col.gameObject.GetComponent<InversedInkHandler>();
        if (InvHandler != null)
        {
            Give(col);
            return;
        }
        InvHandler = col.gameObject.GetComponentInChildren<InversedInkHandler>();
        if (InvHandler != null)
        {
            Give(col);
            return;
        }
    }

    private void Give(Collider col)
    {
        InversedInkHandler InvHandler = col.gameObject.GetComponentInParent<InversedInkHandler>();
        if (InvHandler != null)
        {
            if (InvHandler.AmountOfTime >= InvHandler.FullAmountOfTime)
            {
                //suckSFX.Stop();
                return;
            }
            timeController.AddTime(-TimePerAdd);
            //suckSFX.Play();
            InvHandler.AmountOfTime += TimePerAdd;
            InvHandler.AmountOfTime = Mathf.Clamp(InvHandler.AmountOfTime, 0, InvHandler.FullAmountOfTime);
            //ColorAddition();

            for (int i = 0; i < 1; ++i)
            {
                bool Right = 玩家动画.朝向右方;
                Vector3 RandomPoint = Right ? EulerToDirection(UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(75, 105)) : EulerToDirection(UnityEngine.Random.Range(230, 130), UnityEngine.Random.Range(75, 105));
                ParticleSystem.EmitParams eprams = new ParticleSystem.EmitParams();
                eprams.velocity = RandomPoint * 5;
                eprams.position = Vector3.zero;
                eprams.startSize = 1;
                eprams.startLifetime = InkParticleSystem.startLifetime / 5 * InvHandler.ParticleEffectScale;
                eprams.startColor = new Color(InkParticleSystem.main.startColor.color.r, InkParticleSystem.main.startColor.color.g, InkParticleSystem.main.startColor.color.r, 1);
                InkParticleSystem.Emit(eprams, 1);
            }
            吸墨中 = true;
        }
    }
    private void Suck(Collider col)
    {
        InkHandler handler = col.gameObject.GetComponentInParent<InkHandler>();
        if (handler != null)
        {
            if (handler.AmountOfTime <= 0)
            {
                //suckSFX.Stop();
                return;
            }
            timeController.AddTime(TimePerAdd);
            //suckSFX.Play();
            handler.AmountOfTime -= TimePerAdd;
            handler.AmountOfTime = Mathf.Clamp(handler.AmountOfTime, 0, float.MaxValue);
            //ColorAddition();

            for (int i = 0; i < 1; ++i)
            {
                bool Right = 玩家动画.朝向右方;
                Vector3 RandomPoint = Right?EulerToDirection(UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(75, 105)): EulerToDirection(UnityEngine.Random.Range(230,130), UnityEngine.Random.Range(75, 105));
                ParticleSystem.EmitParams eprams = new ParticleSystem.EmitParams();
                eprams.velocity = -RandomPoint * 5;
                eprams.position = (RandomPoint * InkParticleSystem.startLifetime * 1f * handler.ParticleEffectScale);
                eprams.startSize = 1;
                eprams.startLifetime = InkParticleSystem.startLifetime / 5 * handler.ParticleEffectScale;
                eprams.startColor = new Color(InkParticleSystem.main.startColor.color.r, InkParticleSystem.main.startColor.color.g, InkParticleSystem.main.startColor.color.r, 1);
                InkParticleSystem.Emit(eprams, 1);
            }
            吸墨中 = true;
        }
    }

    public Vector3 EulerToDirection(float Elevation, float Heading)
    {
        float elevation = Elevation * Mathf.Deg2Rad;
        float heading = Heading * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(elevation) * Mathf.Sin(heading), Mathf.Sin(elevation), Mathf.Cos(elevation) * Mathf.Cos(heading));
    }
}
