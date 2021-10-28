namespace AI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    
    [ExecuteInEditMode]
    public class AISensor : MonoBehaviour
    {
        [SerializeField] private float _distance = 10f;
        [SerializeField] private float _angle = 30f;
        [SerializeField] private float _height = 1f;
        [SerializeField] private Color _meshColor = Color.blue;
        [SerializeField] private int _scamFrequency = 30;
        [SerializeField] private LayerMask _layerMaskToDetect;
        [SerializeField] private LayerMask _occlusionLayers;
        [SerializeField] private List<GameObject> _gameObjects = new List<GameObject>();
        [SerializeField] private AttackRegistrator _attackRegistrator;

        private Collider[] _colliders = new Collider[50];
        private int _count;
        private float _scanInterval;
        private float _scamTimer;
        private Mesh _mesh;
        
        private void Start()
        {
            _scanInterval = 1f / _scamFrequency;
        }

        private void Update()
        {
            _scamTimer -= Time.deltaTime;

            if (_scamTimer < 0)
            {
                _scamTimer += _scanInterval;
                Scan();
            }
        }

        private void Scan()
        {
            _count = Physics.OverlapSphereNonAlloc(transform.position, _distance, _colliders, _layerMaskToDetect,
                QueryTriggerInteraction.Collide);
            
            _gameObjects.Clear();

            for (int i = 0; i < _count; i++)
            {
                GameObject obj = _colliders[i].gameObject;
                if (IsInSight(obj))
                {
                    _gameObjects.Add(obj);
                    print("Found");
                    if (obj.GetComponent<StarterAssetsInputs>() != null)
                    {
                        _attackRegistrator.AttackData.Target = obj.transform;
                    }
                }
            }
        }

        public bool IsInSight(GameObject obj)
        {
            Vector3 origin = transform.position;
            Vector3 dest = obj.transform.position;
            Vector3 direction = dest - origin;

            if (direction.y < 0 || direction.y > _height)
                return false;

            direction.y = 0;
            float deltaAngle = Vector3.Angle(direction, transform.forward);
            
            if (deltaAngle > _angle)
                return false;

            origin.y += _height / 2;
            dest.y = origin.y;
            if (Physics.Linecast(origin, dest, _occlusionLayers))
                return false;
            
            return true;
        }
        
        private Mesh CreateWedgeMesh()
        {
            Mesh mesh = new Mesh();

            
            int numTriangles = 8;
            int numVertices = numTriangles * 3;

            int[] triangles = new int[numVertices];
            Vector3[] vertices = new Vector3[numVertices];
            
            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0, -_angle, 0) * Vector3.forward * _distance;
            Vector3 bottomRight = Quaternion.Euler(0, _angle, 0) * Vector3.forward * _distance;

            Vector3 topCenter = bottomCenter + Vector3.up * _height;
            Vector3 topRight = bottomRight + Vector3.up * _height;
            Vector3 topLeft = bottomLeft + Vector3.up * _height;

            int vert = 0;
            //right
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;

            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;
            //left
            vertices[vert++] = bottomCenter;
            vertices[vert++] = topCenter;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;
            //side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;
            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;
            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            for (int i = 0; i < numVertices; ++i)
            {
                triangles[i] = i;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }

        private void OnValidate()
        {
            _mesh = CreateWedgeMesh();
            _scanInterval = 1f / _scamFrequency;
        }

        private void OnDrawGizmos()
        {
            if (_mesh)
            {
                Gizmos.color = _meshColor;
                Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
            }
            
            Gizmos.DrawWireSphere(transform.position, _distance);
            for (int i = 0; i  < _count; i++)
            {
                Gizmos.DrawSphere(_colliders[i].transform.position, .2f);
            }

            Gizmos.color = Color.green;
            foreach (var o in _gameObjects)
            {
                Gizmos.DrawSphere(o.transform.position, .2f);
            }
        }
    }
}