using UnityEngine;
using System.Collections;

public class PerformanceOptimizer : MonoBehaviour {

    


    void Update () {
        // AQUI DICEN QUE, SI USAS EL ACELEROMETRO, PARA QUE LA ORIENTACION DEL DEVICE SEA MAS FIABLE TIENES QUE
        // DESCARTAR LA LECTURA EN CASO DE QUE LA MAGNITUD DEL VECTOR NO ESTÉ ENTRE 0.9 Y 1.1, APROX.
        // O, DIGO YO, SENCILLAMENTE PODRIAS DARLE MENOS PESO.
        // http://stackoverflow.com/questions/23713460/head-tracking-angle-using-accelerometer
    }


    void OnGUI()
    {
    
    }

    
}

