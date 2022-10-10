namespace AlgoriaCore.Application.BaseClases.Dto
{
    public class ComboboxItemDto : ComboboxItemDto<string>
    {
        public ComboboxItemDto() : base() { }

        /// <summary>
        /// Crea un nuevo ComboboxItemDto
        /// </summary>
        /// <param name="value">Valor del elemento</param>
        /// <param name="label">Texto a desplegar del elemento</param>
        public ComboboxItemDto(string value, string label) : base(value, label)
        {
        }
    }

    public class ComboboxItemDto<T>
    {
        /// <summary>
        /// Valor del elemento
        /// </summary>
        public T Value { get; set; }

        public string Label { get; set; }

        public ComboboxItemDto() { }

        /// <summary>
        /// Crea un nuevo ComboboxItemDto
        /// </summary>
        /// <param name="value">Valor del elemento</param>
        /// <param name="label">Texto a desplegar del elemento</param>
        public ComboboxItemDto(T value, string label)
        {
            this.Value = value;
            this.Label = label;
        }
    }
}
