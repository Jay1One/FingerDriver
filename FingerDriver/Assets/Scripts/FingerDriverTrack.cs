using System.Collections.Generic;
using UnityEngine;

public class FingerDriverTrack : MonoBehaviour
{
    private struct TrackSegment
    {
        public Vector3[] Points;
        //проходил ли уже игрок через этот сегмент
        public bool CrossedByPlayer;
        public bool IsPointInSegment(Vector3 point)
        {
            return MathfTriangles.IsPointInTriangleXY(point, Points[0], Points[1], Points[2]);
        }
    }

    [SerializeField] private LineRenderer m_lineRenderer;
    [SerializeField] private bool m_viewDebug;
    [SerializeField] private Transform m_playerPosition;

    private Vector3[] corners;
    private TrackSegment[] segments;
    //счетчик сегментов, которые прошел игрок
    public int SegmentsPassed=0;
    //последний сегмент, в котором был игрок
    private int currentSegment;

    private void Start()
    {
        //Заполняем масив опорных точек трассы
        corners = new Vector3[transform.childCount];
        for (int i = 0; i < corners.Length; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            corners[i] = obj.transform.position;
            obj.GetComponent<MeshRenderer>().enabled =
                false; //здесь отключаем рендеринг опорных точек, можно и сами объекты спрятать
        }

        //настраиваем LineRenderer
        m_lineRenderer.positionCount = corners.Length;
        m_lineRenderer.SetPositions(corners);

        //из полученого LineRenderer запекаем меш
        Mesh mesh = new Mesh();
        m_lineRenderer.BakeMesh(mesh, true);

        //создаем массив сегментов трассы (каждый треугольник описан 3-мя вершинами из массива вершин)
        segments = new TrackSegment[mesh.triangles.Length / 3];
        int segmentCounter = 0;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            segments[segmentCounter] = new TrackSegment();
            segments[segmentCounter].Points = new Vector3[3];
            segments[segmentCounter].Points[0] = mesh.vertices[mesh.triangles[i]];
            segments[segmentCounter].Points[1] = mesh.vertices[mesh.triangles[i + 1]];
            segments[segmentCounter].Points[2] = mesh.vertices[mesh.triangles[i + 2]];
            segmentCounter++;
        }
        //Выставляем начальный сегмент игрока
        SetStartingPlayersegment();
        

        //отдельно можно продебажить что все точки стали ровно там где нужно и увидеть что треугольники генерируются по порядку
        if (!m_viewDebug)
            return;


        foreach (var segment in segments)
        {
            foreach (var pt in segment.Points)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = pt;
                sphere.transform.localScale = Vector3.one * 0.1f;
            }
        }
    }

    /// <summary>
    /// Определяем находится ли точка в переделах трассы
    /// </summary>
    /// <param name="point">точка</param>
    /// <returns></returns>
    public bool IsPointInTrack(Vector3 point)
    {
        //сначала проверяем последний сегмент, в котором был игрок
        if (segments[currentSegment].IsPointInSegment(point))
            {
                //проверяем был ли уже игрок в этом сегменте
                if (!segments[currentSegment].CrossedByPlayer)
                {
                    segments[currentSegment].CrossedByPlayer=true;
                    SegmentsPassed++;
                }
                return true;
            }
        //проверяем следующий сегмент, если он есть
        if (currentSegment < segments.Length - 1)
            {
                if (segments[currentSegment+1].IsPointInSegment(point))
                {
                    currentSegment++;
                    //проверяем был ли уже игрок в этом сегменте
                    if (!segments[currentSegment].CrossedByPlayer)
                    {
                        segments[currentSegment].CrossedByPlayer=true;
                        SegmentsPassed++;
                    }
                    return true;
                }

            }
        //проверяем предыдущий сегмент, если он есть
            if (currentSegment !=0)
            {
                if (segments[currentSegment-1].IsPointInSegment(point))
                {
                    currentSegment++;
                    //проверяем был ли уже игрок в этом сегменте
                    if (!segments[currentSegment].CrossedByPlayer)
                    {
                        segments[currentSegment].CrossedByPlayer=true;
                        SegmentsPassed++;
                    }
                    return true;
                }

            }

            return false;
    }
    
    public void SetStartingPlayersegment()
    {
        for (int i=0; i<segments.Length; i++)
        {
            if (segments[i].IsPointInSegment(m_playerPosition.position))
            {
                currentSegment = i;
            }
        }
        
    }
}


        