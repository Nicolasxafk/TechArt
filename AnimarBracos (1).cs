// ============================================================
// AnimarBracos.cs
// Coloque este script no GameObject raiz do Coelho e arraste os
// bracos nos campos abaixo. Funciona com:
//   - ossos (Bone.007 / Bone.009), ou
//   - bracos separados do corpo (qualquer objeto/Transform).
//
// REQUISITOS PARA BRACOS SEPARADOS:
//   1. Cada braco precisa ser FILHO do Coelho (ou do corpo) na
//      Hierarchy, senao ele nao acompanha o personagem andando.
//   2. O PIVOT do braco precisa estar no OMBRO. Se o pivot estiver
//      no centro do braco, ele gira em torno do meio em vez do ombro.
//      (Solucao: crie um objeto vazio no ombro, coloque o braco dentro
//       dele e arraste o objeto vazio aqui.)
// ============================================================

using UnityEngine;

public class AnimarBracos : MonoBehaviour
{
    [Header("Bracos (osso ou objeto separado)")]
    public Transform bracoEsquerdo;
    public Transform bracoDireito;

    [Header("Ajuste fino - use se o braco comecar torto")]
    public Vector3 offsetEsquerdo = Vector3.zero;
    public Vector3 offsetDireito = Vector3.zero;

    // pose original de cada braco (guardada ao iniciar)
    private Quaternion _repousoEsquerdo = Quaternion.identity;
    private Quaternion _repousoDireito = Quaternion.identity;

    void Start()
    {
        if (bracoEsquerdo != null) _repousoEsquerdo = bracoEsquerdo.localRotation;
        if (bracoDireito != null) _repousoDireito = bracoDireito.localRotation;
    }

    // LateUpdate roda DEPOIS do Animator. Se os bracos ainda forem ossos
    // animados (Idle, Walk...), o Animator sobrescreveria a rotacao no
    // Update comum e o braco nao responderia ao sensor.
    void LateUpdate()
    {
        if (!ArduinoInput.Conectado) return; // sem Arduino, deixa a animacao normal

        if (bracoEsquerdo != null)
        {
            bracoEsquerdo.localRotation = _repousoEsquerdo * Quaternion.Euler(
                ArduinoInput.Pitch1 + offsetEsquerdo.x,
                offsetEsquerdo.y,
                ArduinoInput.Roll1 + offsetEsquerdo.z);
        }

        if (bracoDireito != null)
        {
            bracoDireito.localRotation = _repousoDireito * Quaternion.Euler(
                ArduinoInput.Pitch2 + offsetDireito.x,
                offsetDireito.y,
                ArduinoInput.Roll2 + offsetDireito.z);
        }
    }
}
