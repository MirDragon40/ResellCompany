using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

// NavMeshSurface 컴포넌트가 필요하므로, 해당 컴포넌트를 자동으로 추가하도록 설정한다.
[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshSurfaceBaker : MonoBehaviour
{
    private NavMeshSurface surface;

    void Start()
    {
        // NavMeshSurface 컴포넌트를 가져온다.
        surface = GetComponent<NavMeshSurface>();

        // NavMesh를 굽는다.
        StartCoroutine(BakeNavMesh_Coroutine());
    }

    private IEnumerator BakeNavMesh_Coroutine()
    {

        yield return new WaitForSeconds(0.05f);
        if (surface != null)
        {
            // NavMeshSurface 컴포넌트의 BuildNavMesh 메소드를 호출하여 NavMesh를 굽는다.
            surface.BuildNavMesh();
        }
    }
}
