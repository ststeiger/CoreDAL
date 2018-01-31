
namespace CoreDb
{


    partial class ReadDAL
    {



        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T ConvertResult<T>(object objReturnValue, bool throwOnAssignNullToNonNullableType)
        {
            System.Type tReturnType = typeof(T);

            System.Reflection.TypeInfo ti = System.Reflection.IntrospectionExtensions.GetTypeInfo(tReturnType);
            bool typeIsNullable = (ti.IsGenericType && object.ReferenceEquals(tReturnType.GetGenericTypeDefinition(), typeof(System.Nullable<>)));
            bool typeCanBeAssignedNull = !ti.IsValueType || typeIsNullable;

            if (typeIsNullable)
            {
                tReturnType = System.Nullable.GetUnderlyingType(tReturnType);
            } // End if (typeIsNullable) 


            // Catch & Normalize NULL 
            if (objReturnValue == null || objReturnValue == System.DBNull.Value)
            {
                if (typeCanBeAssignedNull)
                    return (T)(object)null;

                if (throwOnAssignNullToNonNullableType)
                    throw new System.IO.InvalidDataException("ConvertResult cannot assign NULL to non-nullable type.");

                return default(T);
            } // End if (objReturnValue == null) 


            System.Type tDataTypeOfScalarResult = objReturnValue.GetType();
            if (object.ReferenceEquals(tReturnType, tDataTypeOfScalarResult))
                return (T)objReturnValue;

            string strReturnValue = System.Convert.ToString(objReturnValue, System.Globalization.CultureInfo.InvariantCulture);

            try
            {
                if (object.ReferenceEquals(tReturnType, typeof(object)))
                {
                    return (T)objReturnValue;
                }
                else if (object.ReferenceEquals(tReturnType, typeof(string)))
                {
                    return (T)(object)strReturnValue;
                } // End if string
                else if (object.ReferenceEquals(tReturnType, typeof(bool)))
                {
                    bool bReturnValue = false;

                    if (bool.TryParse(strReturnValue, out bReturnValue))
                        return (T)(object)bReturnValue;

                    if (strReturnValue == "0")
                        return (T)(object)false;

                    if (strReturnValue == "0.0")
                        return (T)(object)false;

                    return (T)(object)true;
                } // End if bool
                else if (object.ReferenceEquals(tReturnType, typeof(int)))
                {
                    int iReturnValue;
                    if (int.TryParse(strReturnValue, out iReturnValue))
                        return (T)(object)iReturnValue;

                    throw new System.IO.InvalidDataException("ConvertResult: Returned data \"" + strReturnValue + "\" is not a valid integer.");
                } // End if int
                else if (object.ReferenceEquals(tReturnType, typeof(uint)))
                {
                    uint uiReturnValue;
                    if (uint.TryParse(strReturnValue, out uiReturnValue))
                        return (T)(object)uiReturnValue;

                    throw new System.IO.InvalidDataException("ConvertResult: Returned data \"" + strReturnValue + "\" is not a valid unsigned integer.");
                } // End if uint
                else if (object.ReferenceEquals(tReturnType, typeof(long)))
                {
                    long lngReturnValue;
                    if (long.TryParse(strReturnValue, out lngReturnValue))
                        return (T)(object)lngReturnValue;

                    throw new System.IO.InvalidDataException("ConvertResult: Returned data \"" + strReturnValue + "\" is not a valid unsigned integer.");
                } // End if long
                else if (object.ReferenceEquals(tReturnType, typeof(ulong)))
                {
                    ulong ulngReturnValue;
                    if (ulong.TryParse(strReturnValue, out ulngReturnValue))
                        return (T)(object)ulngReturnValue;

                    throw new System.IO.InvalidDataException("ConvertResult: Returned data \"" + strReturnValue + "\" is not a valid unsigned long.");
                } // End if ulong
                else if (object.ReferenceEquals(tReturnType, typeof(float)))
                {
                    float fltReturnValue;
                    if (float.TryParse(strReturnValue, out fltReturnValue))
                        return (T)(object)fltReturnValue;

                    throw new System.IO.InvalidDataException("ConvertResult: Returned data \"" + strReturnValue + "\" is not a valid float.");
                }
                else if (object.ReferenceEquals(tReturnType, typeof(double)))
                {
                    double dblReturnValue;
                    if (double.TryParse(strReturnValue, out dblReturnValue))
                        return (T)(object)dblReturnValue;

                    throw new System.IO.InvalidDataException("ConvertResult: Returned data \"" + strReturnValue + "\" is not a valid double.");
                }
                else if (object.ReferenceEquals(tReturnType, typeof(System.Net.IPAddress)))
                {
                    System.Net.IPAddress ipAddress = null;

                    if (System.Net.IPAddress.TryParse(strReturnValue, out ipAddress))
                        return (T)(object)ipAddress;

                    throw new System.IO.InvalidDataException("ConvertResult: Returned data \"" + strReturnValue + "\" is not a valid IP address.");
                } // End if IPAddress
                else if (object.ReferenceEquals(tReturnType, typeof(System.Guid)))
                {
                    System.Guid retUID;
                    if (System.Guid.TryParse(strReturnValue, out retUID))
                        return (T)(object)retUID;

                    throw new System.IO.InvalidDataException("ConvertResult: Returned data \"" + strReturnValue + "\" is not a valid GUID.");
                } // End if System.Guid
                else // No datatype matches
                {
                    throw new System.NotImplementedException("ConvertResult<T>: No implicit conversion operation defined for type \"" + tReturnType.FullName + "\".");
                } // End else of if tReturnType = datatype

            } // End Try
            catch (System.Exception ex)
            {
                if (Log(ex))
                    throw;
            } // End Catch

            return default(T);
        }

    }
}
