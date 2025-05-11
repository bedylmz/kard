using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CardEffects : MonoBehaviour
{
    private WaitForSecondsRealtime oneSec;

    private GameObject playerHealthText;
    private GameObject playerBrainText;
    private GameObject opponentHealthText;
    private GameObject opponentBrainText;

    public int brainDamage;
    public int brainHeal; 
    public int healthDamage;
    public int healthHeal;

    public bool both;

    public float healthHealTime;
    public float brainHealTime;
    public bool  drainIsHeal;

    private bool lensDistortionOut;
    private bool lensDistortionIn;
    private bool lensDistortionNormal;

    private bool UnFocused;
    private bool Focused;

    private bool ChromaticAberrationRise;
    private bool ChromaticAberrationFall;

    private bool BloomRise;
    private bool BloomFall;

    private bool VignetteRise;
    private bool VignetteFall;

    private GameObject postProcessing;

    public Sprite sprite;

    private PostProcessProfile profile;

    public string description;

    // Start is called before the first frame update
    void Start()
    {
        oneSec = new WaitForSecondsRealtime(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(int who)
    {
        opponent opponent = GameObject.Find("opponent").GetComponent<opponent>();
        player player = GameObject.Find("hand").GetComponent<player>();

        playerHealthText = GameObject.Find("playerHealthText");
        playerBrainText = GameObject.Find("playerBrainText");
        opponentHealthText = GameObject.Find("opponentHealthText");
        opponentBrainText = GameObject.Find("opponentBrainText");

    float oldPlayerHealth = player.Health;
        float oldOpponentHealth = opponent.Health;

        float oldPlayerBrain = player.Brain;
        float oldOpponentBrain = opponent.Brain;

        if (brainDamage > 0) brainDamage = Random.Range(1, brainDamage);
        if (brainHeal > 0) brainHeal = Random.Range(1, brainHeal);
        if (healthDamage > 0) healthDamage = Random.Range(1, healthDamage);
        if (healthHeal > 0) healthHeal = Random.Range(1, healthHeal);

        if (brainDamage < 0) brainDamage = Random.Range(brainDamage, -1);
        if (brainHeal < 0) brainHeal = Random.Range(brainHeal, -1);
        if (healthDamage < 0) healthDamage = Random.Range(healthDamage, -1);
        if (healthHeal < 0) healthHeal = Random.Range(healthHeal, -1);
        if (who == 0)//played by player
        {
            
            opponent.Health -= healthDamage;
            opponent.Brain -= brainDamage;
            player.Brain += brainHeal;
            player.Health += healthHeal;

            //effect both side
            if (both)
            {
                player.Health -= healthDamage;
                player.Brain -= brainDamage;
                opponent.Brain += brainHeal;
                opponent.Health += healthHeal;
            }

            //drain health of brainü
            if(healthHealTime != 0)
            {
                StartCoroutine(playerHealer(player, false, drainIsHeal, healthHealTime));
            }
            if (brainHealTime != 0)
            {
                StartCoroutine(playerHealer(player, false, drainIsHeal, healthHealTime));
            }

        }
        else
        {
            player.Health -= healthDamage;
            player.Brain -= brainDamage;
            opponent.Brain += brainHeal;
            opponent.Health += healthHeal;

            //effect both side
            if (both)
            {
                opponent.Health -= healthDamage;
                opponent.Brain -= brainDamage;
                player.Brain += brainHeal;
                player.Health += healthHeal;
            }

            //drain health of brainü
            if (healthHealTime != 0)
            {
                StartCoroutine(opponentHealer(opponent, false, drainIsHeal, healthHealTime));
            }
            if (brainHealTime != 0)
            {
                StartCoroutine(opponentHealer(opponent, false, drainIsHeal, healthHealTime));
            }
        }

        bool[] curseList = { lensDistortionOut, lensDistortionIn, UnFocused, ChromaticAberrationRise, BloomRise, VignetteRise };
        bool[] cureList = {  lensDistortionNormal, Focused, ChromaticAberrationFall, BloomFall, VignetteFall };
        for(int i = 0; i < curseList.Length; i++) { curseList[i] = false; }
        for(int i = 0; i < cureList.Length; i++) { cureList[i] = false; }

        int effect = Random.Range(0, 3);
        if (effect == 0)
        {
            curseList[Random.Range(0, curseList.Length)] = true;
        }
        else if (effect == 1)
        {
            cureList[Random.Range(0, cureList.Length)] = true;
        }
        else if(effect == 2)
        {
            //nothing
        }

        //common
        postProcessing = GameObject.Find("postProcessing");
        profile = postProcessing.GetComponent<PostProcessVolume>().profile;

        //Lens
        if (curseList[0])
        {
            // Check if Bloom exists
            if (!profile.TryGetSettings(out LensDistortion lens))
            {
                // If not, add Bloom
                lens = profile.AddSettings<LensDistortion>();
            }

            // Set properties
            StartCoroutine(LensDistortion(lens, 50, 2));
            //StartCoroutine(LoopDistortion(lens, 5));
        }
        else if (curseList[1])
        {
            // Check if Bloom exists
            if (!profile.TryGetSettings(out LensDistortion lens))
            {
                // If not, add Bloom
                lens = profile.AddSettings<LensDistortion>();
            }

            // Set properties
            //lens.intensity.Override(-50);
            StartCoroutine(LensDistortion(lens, -50, 2));
        }
        else if (cureList[0])
        {
            // Check if Bloom exists
            if (!profile.TryGetSettings(out LensDistortion lens))
            {
                // If not, add Bloom
                lens = profile.AddSettings<LensDistortion>();
            }

            // Set properties
            //lens.intensity.Override(-50);
            StartCoroutine(LensDistortion(lens, 0, 2));
        }

        //Depth of Field
        if (curseList[2])
        {
            // Check if exists
            if (!profile.TryGetSettings(out DepthOfField depth))
            {
                depth = profile.AddSettings<DepthOfField>();
            }

            // Set properties
            StartCoroutine(focusShift(depth, 0.1f, 2));
        }
        else if (cureList[1])
        {
            // Check if exists
            if (!profile.TryGetSettings(out DepthOfField depth))
            {
                depth = profile.AddSettings<DepthOfField>();
            }

            // Set properties
            StartCoroutine(focusShift(depth, 10, 2));
        }

        //Chormatic Abbration
        if (curseList[3])
        {
            // Check if exists
            if (!profile.TryGetSettings(out ChromaticAberration setting))
            {
                setting = profile.AddSettings<ChromaticAberration>();
            }

            // Set properties
            StartCoroutine(aberration(setting, 0.53f, 2));
        }
        else if (cureList[2])
        {
            // Check if exists
            if (!profile.TryGetSettings(out ChromaticAberration setting))
            {
                setting = profile.AddSettings<ChromaticAberration>();
            }

            // Set properties
            StartCoroutine(aberration(setting, 0, 2));
        }

        //Bloom
        if (curseList[4])
        {
            // Check if exists
            if (!profile.TryGetSettings(out Bloom setting))
            {
                setting = profile.AddSettings<Bloom>();
            }

            // Set properties
            StartCoroutine(bloom(setting, 0.54f, 42f, 0.81f, 4.27f, 2));
        }
        else if (cureList[3])
        {
            // Check if exists
            if (!profile.TryGetSettings(out Bloom setting))
            {
                setting = profile.AddSettings<Bloom>();
            }

            // Set properties
            StartCoroutine(bloom(setting, 0f, 0f, 0.5f, 7f, 2));
        }

        //Vignette
        if (curseList[5])
        {
            // Check if exists
            if (!profile.TryGetSettings(out Vignette setting))
            {
                setting = profile.AddSettings<Vignette>();
            }

            // Set properties
            StartCoroutine(vignette(setting, 0.73f, 2));
        }
        else if (cureList[4])
        {
            // Check if exists
            if (!profile.TryGetSettings(out Vignette setting))
            {
                setting = profile.AddSettings<Vignette>();
            }

            // Set properties
            StartCoroutine(vignette(setting, 0, 2));
        }

        if(oldPlayerHealth > player.Health)
        {
            StartCoroutine(TextColor(playerHealthText, Color.red));
        }
        else if (oldPlayerHealth < player.Health)
        {
            StartCoroutine(TextColor(playerHealthText, Color.green));

        }
        if (oldPlayerBrain > player.Brain)
        {
            StartCoroutine(TextColor(playerBrainText, Color.red));

        }
        else if (oldPlayerBrain < player.Brain)
        {
            StartCoroutine(TextColor(playerBrainText, Color.red));

        }

        if (oldOpponentHealth > opponent.Health)
        {
            StartCoroutine(TextColor(opponentHealthText, Color.red));
        }
        else if (oldOpponentHealth < opponent.Health)
        {
            StartCoroutine(TextColor(opponentHealthText, Color.green));

        }
        if (oldOpponentBrain > opponent.Brain)
        {
            StartCoroutine(TextColor(opponentBrainText, Color.red));

        }
        else if (oldOpponentBrain < opponent.Brain)
        {
            StartCoroutine(TextColor(opponentBrainText, Color.green));

        }

    }

    public IEnumerator TextColor(GameObject obj, Color color)
    {
        obj.GetComponent<TMP_Text>().color = color;
        yield return new WaitForSecondsRealtime(2f);
        obj.GetComponent<TMP_Text>().color = Color.white;

    }

    public IEnumerator LensDistortion(LensDistortion lens, float target, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / (duration));

            // Apply ease-in (quadratic)
            float easeInT = t;

            lens.intensity.Override(Mathf.Lerp(lens.intensity, target, easeInT));

            yield return null;
        }
        lens.intensity.Override(target);

    }
    public IEnumerator LoopDistortion(LensDistortion lens, int loop)
    {
        float target = 0;
        for(int i = 0; i < loop; i++)
        {
            if (i % 3 == 0) { target = 50; }
            else if (i % 3 == 0) { target = 0; }
            else { target = -50; }
            StartCoroutine(LensDistortion(lens, target, 4));
            yield return new WaitForSeconds(4);
        }
        profile.RemoveSettings<LensDistortion>();
    }

    public IEnumerator focusShift(DepthOfField depth, float target, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / (duration));

            // Apply ease-in (quadratic)
            float easeInT = t;

            depth.focusDistance.Override(Mathf.Lerp(depth.focusDistance, target, easeInT));

            yield return null;
        }
        depth.focusDistance.Override(target);

    }

    public IEnumerator aberration(ChromaticAberration setting, float target, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / (duration));

            // Apply ease-in (quadratic)
            float easeInT = t;

            setting.intensity.Override(Mathf.Lerp(setting.intensity, target, easeInT));

            yield return null;
        }
        setting.intensity.Override(target);

    }

    public IEnumerator bloom(Bloom setting, float threshold, float intensity, float softKnee, float diffusion, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / (duration));

            // Apply ease-in (quadratic)
            float easeInT = t;

            setting.intensity.Override(Mathf.Lerp(setting.intensity, intensity, easeInT));
            setting.threshold.Override(Mathf.Lerp(setting.threshold, threshold, easeInT));
            setting.softKnee.Override(Mathf.Lerp(setting.softKnee, softKnee, easeInT));
            setting.diffusion.Override(Mathf.Lerp(setting.diffusion, diffusion, easeInT));

            yield return null;
        }
        setting.intensity.Override(intensity);
        setting.threshold.Override(threshold);
        setting.softKnee.Override(softKnee);
        setting.diffusion.Override(diffusion);

    }

    public IEnumerator vignette(Vignette setting, float target, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / (duration));

            // Apply ease-in (quadratic)
            float easeInT = t;

            setting.intensity.Override(Mathf.Lerp(setting.intensity, target, easeInT));

            yield return null;
        }
        setting.intensity.Override(target);

    }

    public IEnumerator playerHealer(player player, bool isBrain, bool isHeal, float duration)
    {

        for (int i = 0; i < duration; i++)
        {
            yield return oneSec;
            if (isBrain)
            {
                if (isHeal) player.Brain += 1;
                else player.Brain -= 1;
            }
            else
            {
                if (isHeal) player.Health += 1;
                else player.Health -= 1;
            }
        }
    }

    public IEnumerator opponentHealer(opponent opponent, bool isBrain, bool isHeal, float duration)
    {

        for (int i = 0; i < duration; i++)
        {
            yield return oneSec;
            if (isBrain)
            {
                if (isHeal) opponent.Brain += 1;
                else opponent.Brain -= 1;
            }
            else
            {
                if (isHeal) opponent.Health += 1;
                else opponent.Health -= 1;
            }
        }
    }

}
