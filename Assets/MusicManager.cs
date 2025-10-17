
using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Tooltip("Seconds to crossfade between anchors.")]
    public float fadeDuration = 1.0f;

    private AudioSource _currentSource;
    private float _currentTargetVolume = 1f;
    private Coroutine _fadeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Fade out the currently playing source (if any) and fade in the new one.
    /// </summary>
    public void PlayFrom(AudioSource newSource, bool restart = true)
    {
        if (newSource == null) return;

        // If it's the same source, just ensure it's playing.
        if (_currentSource == newSource)
        {
            if (!newSource.isPlaying)
                newSource.Play();
            return;
        }

        // Start the new source at zero volume so we can fade it in.
        _currentTargetVolume = newSource.volume;
        if (restart) newSource.Stop();
        newSource.Play();
        newSource.volume = 0f;

        // Stop any previous fade and start a new crossfade.
        if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(Crossfade(_currentSource, newSource, fadeDuration));

        _currentSource = newSource;
    }

    private IEnumerator Crossfade(AudioSource from, AudioSource to, float duration)
    {
        float t = 0f;
        float fromStart = from ? from.volume : 0f;
        float toEnd = _currentTargetVolume;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = duration <= 0f ? 1f : Mathf.Clamp01(t / duration);

            if (from) from.volume = Mathf.Lerp(fromStart, 0f, k);
            if (to)   to.volume   = Mathf.Lerp(0f, toEnd, k);

            yield return null;
        }

        if (from)
        {
            from.Stop();
            from.volume = fromStart; // restore its original setting
        }

        if (to) to.volume = toEnd;
        _fadeRoutine = null;
    }
}
