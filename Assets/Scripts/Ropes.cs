using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class Ropes : MonoBehaviour
{
	public event Action onCut; 

	[SerializeField] private CutRope[] _gameObjects;

	[SerializeField] private GameObject m_topRope;
	[SerializeField] private GameObject m_plateform;

	private List<CharacterJoint> _jointsParents = new List<CharacterJoint>();
    private List<CharacterJoint> _jointsChild = new List<CharacterJoint>();

    [SerializeField, OnValueChanged("Check")] private CutRope _topLink;
    public CutRope TopLink => _topLink;

    public bool HasBeenCut => m_hasBeenCut;
    private bool m_hasBeenCut;

    public void DestroyAfterTime()
    {
	    if (m_hasBeenCut)
		    return;

	    DisablePlatform();

		onCut?.Invoke();

	    m_hasBeenCut = true;

		StartCoroutine(DestroyAfterTime(5f));
	}
    public void DisablePlatform()
    {
		Vector3 posToFall = new Vector3(m_topRope.transform.position.x, 0, m_topRope.transform.position.z);
	    m_topRope.transform.DOMove(posToFall, 1.5f).SetEase(Ease.InExpo);
	    m_plateform.SetActive(false);
	}

	private IEnumerator DestroyAfterTime(float _delay)
    {
		this.transform.parent = null;

	    yield return new WaitForSeconds(2);

		foreach (CutRope cutRope in _gameObjects)
	    {
		    if (cutRope.Rigidbody) cutRope.Rigidbody.drag = 100;
	    }

		yield return new WaitForSeconds(_delay - 2f);

		Destroy(this.gameObject);
    }

    void Check()
    {
	    if (_topLink.name != "Link 1")
	    {
		    Debug.LogError("Wrong Link assign to variable (topLink) or click on RenameLink and retry");

		    _topLink = null;
		}
	}

	[Button]
	void RenameLink()
    {
	    for (int i = 0; i < _gameObjects.Length; i++)
	    {
		    _gameObjects[i].name = $"Link {i + 1}";
	    }
    }

    void AddCharacterBodyParent()
    {
	    for (int i = 1; i < _gameObjects.Length; i++)
	    {
		    if (_gameObjects[i].GetComponent<CharacterJoint>() == false)
		    {
				_jointsParents.Add(_gameObjects[i].AddComponent<CharacterJoint>());
			}
		}

	    foreach (CharacterJoint characterJoint in _jointsParents)
	    {
		    characterJoint.connectedBody = characterJoint.transform.parent.GetComponent<Rigidbody>();
		}
	}

    void AddCharacterBodyChild ()
    {
	    for (int i = 0; i < _gameObjects.Length - 1; i++)
	    {
			_jointsChild.Add(_gameObjects[i].AddComponent<CharacterJoint>());
	    }

	    foreach (CharacterJoint characterJoint in _jointsChild)
	    {
		    characterJoint.connectedBody = characterJoint.transform.parent.GetComponent<Rigidbody>();
	    }
    }
}
