using UnityEngine;
using System.Collections;

public class PerformanceOptimizer : MonoBehaviour {

    


    void Update () {
        /*
	    Comprobar los fotogramas por segundo
        Si en los ultimos segundos baja de minAllowedFPS:
            QualitySettings.SetQualityLevel(index, bool applyExpensiveChanges) QualitySettings.names
            QualitySettings.vSyncCount
            Reducir calidad de sombra o desactivarla:
                QualitySettings.shadowProjection
                No hay forma de cambiar la resolucion del shadowmap por script
            Desactivar cubemaps de reflexion y usar color simple
            Desactivar cubemap de skybox y usar color simple
            Bajar el fixed timestep
            ? Bajar el contact offset
            ? Bajar las collision resolve iterations
            Asignar las mallas de colision lowpoly
        
        Asegurate de que los fps se muestran sin interpolar mucho tiempo, solo el segundo anterior.
        
        Ajustar automaticamente los valores de la sombra segun la posicion de la camara:
            QualitySettings.shadowNearPlaneOffset
            QualitySettings.shadowDistance

        Hacer GUI que permita debugar los valores de performance, con spinners y toggles.

        Hacer profiling a ver dónde se está yendo el performance. No sé si se puede hacer profiling en remoto desde el device.
        */
    }


    void OnGUI()
    {
    
    }

    
}

