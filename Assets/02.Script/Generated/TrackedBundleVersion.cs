using System.Collections;

// GENERATED CODE.
public class TrackedBundleVersion
{
	public static readonly string bundleIdentifier = "FootCraft";

	public static readonly TrackedBundleVersionInfo Version_1_0_0 =  new TrackedBundleVersionInfo ("1.0.0", 0);
	public static readonly TrackedBundleVersionInfo Version_1_0_1 =  new TrackedBundleVersionInfo ("1.0.1", 1);
	
	public static readonly TrackedBundleVersion Instance = new TrackedBundleVersion ();

	public static TrackedBundleVersionInfo Current { get { return Instance.current; } }

	public static int CurrentAndroidBundleVersionCode { get { return 1; } }

	public ArrayList history = new ArrayList ();

	public TrackedBundleVersionInfo current = Version_1_0_0;

	public  TrackedBundleVersion() {
		history.Add (Version_1_0_0);
		history.Add (Version_1_0_1);
	}

}
