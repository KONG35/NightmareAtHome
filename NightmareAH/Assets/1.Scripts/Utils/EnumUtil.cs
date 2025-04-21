using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumUtil
{
    /// <summary>
    /// 대소문자 무시하고 enum 파싱하는 함수
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryParseIgnoreCase<T>(string input, out T result) where T : struct, Enum
    {
        return Enum.TryParse(input, true, out result);
    }
}
