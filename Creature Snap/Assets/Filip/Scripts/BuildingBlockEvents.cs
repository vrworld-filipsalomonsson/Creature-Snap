using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class BuildingBlockEvents : MonoBehaviour
{
	public Rigidbody rb;
	public GameObject cam;
	float previousAngularDrag = 0f;
	float previousMass = 0f;
	public SnapInteractable snapInteractable;
	public Grabbable grabbable;

    void Awake()
	{
        _interactorView = GetComponent<SnapInteractor>();
		InteractorView = _interactorView as IInteractorView;

        WhenSelect.AddListener(OnSelect);
		WhenUnselect.AddListener(OnUnselect);
	}

	void OnSelect()
	{
		rb.useGravity = false;
		rb.isKinematic = true;
        previousAngularDrag = rb.angularDamping;
		rb.angularDamping = 0f;

		previousMass = rb.mass;
		rb.mass = 0f;

		grabbable.TransferOnSecondSelection = true;
        snapInteractable.gameObject.SetActive(true);
		snapInteractable.Enable();
	}

	void OnUnselect()
	{
        rb.angularDamping = previousAngularDrag;

		rb.mass = previousMass;

		rb.isKinematic = false;
		rb.useGravity = true;

        grabbable.TransferOnSecondSelection = false;
        // snapInteractable.Disable();
        //snapInteractable.gameObject.SetActive(false);
    }

	#region Original Script

	/// <summary>
	/// The <see cref="IInteractorView"/> (Interactor) component to wrap.
	/// </summary>
	[Tooltip("The IInteractorView (Interactor) component to wrap.")]
	[Interface(typeof(IInteractorView))]
	private UnityEngine.Object _interactorView;
	private IInteractorView InteractorView;

	/// <summary>
	/// Raised when the Interactor is enabled.
	/// </summary>
	[Tooltip("Raised when the Interactor is enabled.")]
	private UnityEvent _whenEnabled = new UnityEvent();

	/// <summary>
	/// Raised when the Interactor is disabled.
	/// </summary>
	[Tooltip("Raised when the Interactor is disabled.")]
	private UnityEvent _whenDisabled = new UnityEvent();

	/// <summary>
	/// Raised when the Interactor is hovering over an Interactable.
	/// </summary>
	[Tooltip("Raised when the Interactor is hovering over an Interactable.")]
	[SerializeField]
	private UnityEvent _whenHover = new UnityEvent();

	/// <summary>
	/// Raised when the Interactor stops hovering over an Interactable.
	/// </summary>
	[Tooltip("Raised when the stops hovering over an Interactable.")]
	[SerializeField]
	private UnityEvent _whenUnhover = new UnityEvent();

	/// <summary>
	/// Raised when the Interactor selects an Interactable.
	/// </summary>
	[Tooltip("Raised when the Interactor selects an Interactable.")]
	[SerializeField]
	private UnityEvent _whenSelect = new UnityEvent();

	/// <summary>
	/// Raised when the Interactor stops selecting an Interactable.
	/// </summary>
	[Tooltip("Raised when the Interactor stops selecting an Interactable.")]
	[SerializeField]
	private UnityEvent _whenUnselect = new UnityEvent();

	/// <summary>
	/// Raised when the Interactor preprocesses
	/// </summary>
	[Tooltip("Raised when the Interactor preprocesses.")]
	private UnityEvent _whenPreprocessed = new UnityEvent();

	/// <summary>
	/// Raised when the Interactor processes
	/// </summary>
	[Tooltip("Raised when the Interactor processes.")]
	private UnityEvent _whenProcessed = new UnityEvent();
	/// <summary>
	/// Raised when the Interactor processes
	/// </summary>
	[Tooltip("Raised when the Interactor processes.")]
	private UnityEvent _whenPostprocessed = new UnityEvent();

	public UnityEvent WhenDisabled => _whenDisabled;
	public UnityEvent WhenEnabled => _whenEnabled;
	public UnityEvent WhenHover => _whenHover;
	public UnityEvent WhenUnhover => _whenUnhover;
	public UnityEvent WhenSelect => _whenSelect;
	public UnityEvent WhenUnselect => _whenUnselect;

	public UnityEvent WhenPreprocessed => _whenPreprocessed;
	public UnityEvent WhenProcessed => _whenProcessed;
	public UnityEvent WhenPostprocessed => _whenPostprocessed;

	protected bool _started = false;


	void Start()
	{
		this.BeginStart(ref _started);
		this.AssertField(InteractorView, nameof(InteractorView));
		this.EndStart(ref _started);
	}

	void OnEnable()
	{
		if (_started)
		{
			InteractorView.WhenStateChanged += HandleStateChanged;
			InteractorView.WhenPreprocessed += HandlePreprocessed;
			InteractorView.WhenProcessed += HandleProcessed;
			InteractorView.WhenPostprocessed += HandlePostprocessed;
		}
	}

	void OnDisable()
	{
		if (_started)
		{
			InteractorView.WhenStateChanged -= HandleStateChanged;
			InteractorView.WhenPreprocessed -= HandlePreprocessed;
			InteractorView.WhenProcessed -= HandleProcessed;
			InteractorView.WhenPostprocessed -= HandlePostprocessed;
		}
	}

	private void HandleStateChanged(InteractorStateChangeArgs args)
	{
		switch (args.NewState)
		{
			case InteractorState.Disabled:
				_whenDisabled.Invoke();
				break;
			case InteractorState.Normal:
				if (args.PreviousState == InteractorState.Hover)
				{
					_whenUnhover.Invoke();
				}
				else if (args.PreviousState == InteractorState.Disabled)
				{
					_whenEnabled.Invoke();
				}
				break;
			case InteractorState.Hover:
				if (args.PreviousState == InteractorState.Normal)
				{
					_whenHover.Invoke();
				}
				else if (args.PreviousState == InteractorState.Select)
				{
					_whenUnselect.Invoke();
				}

				break;
			case InteractorState.Select:
				if (args.PreviousState == InteractorState.Hover)
				{
					_whenSelect.Invoke();
				}

				break;
		}
	}

	private void HandlePreprocessed()
	{
		_whenPreprocessed.Invoke();
	}

	private void HandleProcessed()
	{
		_whenProcessed.Invoke();
	}

	private void HandlePostprocessed()
	{
		_whenPostprocessed.Invoke();
	}



	#region Inject

	public void InjectAllInteractorUnityEventWrapper(IInteractorView interactorView)
	{
		InjectInteractorView(interactorView);
	}

	public void InjectInteractorView(IInteractorView interactorView)
	{
		_interactorView = interactorView as UnityEngine.Object;
		InteractorView = interactorView;
	}

	#endregion


	#endregion

}
