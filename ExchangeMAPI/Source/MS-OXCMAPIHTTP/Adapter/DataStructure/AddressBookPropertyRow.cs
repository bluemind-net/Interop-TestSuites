namespace Microsoft.Protocols.TestSuites.MS_OXCMAPIHTTP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Protocols.TestSuites.Common;

    /// <summary>
    /// The AddressBookPropertyRow structures a list of property values without including the property tags that correspond to the property values.
    /// </summary>
    public struct AddressBookPropertyRow
    {
        /// <summary>
        /// A byte that indicates whether all property values are present and without error in the ValueArray field.
        /// </summary>
        public byte Flag;

        /// <summary>
        /// An array of variable-sized structures that contains the property values.
        /// </summary>
        public PropertyValue[] ValueArray;

        /// <summary>
        /// Parse the AddressBookPropertyRow structure.
        /// </summary>
        /// <param name="rawBuffer">The raw data returned from server.</param>
        /// <param name="propTagArray">The list of property tags.</param>
        ///  <param name="index">The start index.</param>
        /// <returns>Return an instance of AddressBookPropertyRow.</returns>
        public static AddressBookPropertyRow Parse(byte[] rawBuffer, LargePropTagArray propTagArray, ref int index)
        {
            AddressBookPropertyRow addressBookPropertyRow = new AddressBookPropertyRow();
            addressBookPropertyRow.Flag = rawBuffer[index];
            index++;
            List<PropertyValue> valueArray = new List<PropertyValue>();

            Context.Instance.PropertyBytes = rawBuffer;
            Context.Instance.CurIndex = index;
            Context.Instance.CurProperty = new Property(PropertyType.PtypUnspecified);

            // If the value of the Flags field is set to 0x00: The array contains either a PropertyValue structure, or a TypedPropertyValue structure.
            // If the value of the Flags field is set to 0x01: The array contains either a FlaggedPropertyValue structure, or a FlaggedPropertyValueWithType structure. 
            if (addressBookPropertyRow.Flag == 0x00)
            {
                foreach (PropertyTag propertyTag in propTagArray.PropertyTags)
                {
                    if (propertyTag.PropertyType == 0x0000)
                    {
                        // If the value of the Flags field is set to 0x00: The array contains a TypedPropertyValue structure, if the type of property is PtyUnspecified.
                        TypedPropertyValue typedPropertyValue = new TypedPropertyValue();

                        // Parse the TypedPropertyValue with the instance of the context which contains the datas and start index.
                        typedPropertyValue.Parse(Context.Instance);
                        valueArray.Add(typedPropertyValue);
                        index = Context.Instance.CurIndex;
                    }
                    else
                    {
                        // If the value of the Flags field is set to 0x00: The array contains a PropertyValue structure, if the type of property is specified.
                        Context.Instance.CurProperty.Type = (PropertyType)propertyTag.PropertyType;
                        PropertyValue propertyValue = new PropertyValue();

                        // Parse the TypedPropertyValue with the instance of the context which contains the datas and start index.
                        propertyValue.Parse(Context.Instance);
                        valueArray.Add(propertyValue);
                        index = Context.Instance.CurIndex;
                    }
                }
            }
            else if (addressBookPropertyRow.Flag == 0x01)
            {
                foreach (PropertyTag propertyTag in propTagArray.PropertyTags)
                {
                    if (propertyTag.PropertyType == 0x0000)
                    {
                        // If the value of the Flags field is set to 0x01: The array contains a FlaggedPropertyValueWithType structure, if the type of property is PtyUnspecified.
                        FlaggedPropertyValueWithType flaggedPropertyValue = new FlaggedPropertyValueWithType();

                        // Parse the TypedPropertyValue with the instance of the context which contains the datas and start index.
                        flaggedPropertyValue.Parse(Context.Instance);
                        valueArray.Add(flaggedPropertyValue);
                        index = Context.Instance.CurIndex;
                    }
                    else
                    {
                        // If the value of the Flags field is set to 0x01: The array contains a FlaggedPropertyValue structure, if the type of property is specified.
                        Context.Instance.CurProperty.Type = (PropertyType)propertyTag.PropertyType;
                        FlaggedPropertyValue propertyValue = new FlaggedPropertyValue();

                        // Parse the TypedPropertyValue with the instance of the context which contains the datas and start index.
                        propertyValue.Parse(Context.Instance);
                        valueArray.Add(propertyValue);
                        index = Context.Instance.CurIndex;
                    }
                }
            }

            addressBookPropertyRow.ValueArray = valueArray.ToArray();

            return addressBookPropertyRow;
        }
    }
}