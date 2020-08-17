using System.Collections.Generic;

namespace Constants
{
	public static class ErrorDictionary
	{
		public static Dictionary<(int, string), string> Errors = new Dictionary<(int, string), string>
		{
			[(1, "en-US")] = "Person already exists in the database",							[(1, "ru-RU")] = "Пользователь уже существует",
			[(2, "en-US")] = "Person is not exist in the database",								[(2, "ru-RU")] = "Указанного пользователя не существует",
			[(3, "en-US")] = "Code is not correct",												[(3, "ru-RU")] = "Неверный код",
			[(4, "en-US")] = "Refresh token is invalid",										[(4, "ru-RU")] = "Неверный рефреш-токен",
			[(5, "en-US")] = "Authorization failed",											[(5, "ru-RU")] = "Ошибка авторизации",
			[(6, "en-US")] = "Phone number format is not correct",								[(6, "ru-RU")] = "Неверный формат номера телефона",
			[(7, "en-US")] = "Authorization code format is not correct",						[(7, "ru-RU")] = "Неверный формат кода авторизации",
			[(8, "en-US")] = "Refresh token should not be empty",								[(8, "ru-RU")] = "Рефреш-токен не может быть пустым",
			[(9, "en-US")] = "Image content should not be empty",								[(9, "ru-RU")] = "Изображение не может быть пустым",
			[(10, "en-US")] = "Event is not exist in the database",								[(10, "ru-RU")] = "Событие не существует",
			[(11, "en-US")] = "Chat message is not exist in the database",						[(11, "ru-RU")] = "Сообщение не существует",
			[(12, "en-US")] = "Image with such GUID is not exist",								[(12, "ru-RU")] = "Изображение с таким идентификатором не существует",
			[(13, "en-US")] = "Event status id is not correct",									[(13, "ru-RU")] = "Идентификатор статуса события некорректен",
			[(14, "en-US")] = "Event type id is not correct",									[(14, "ru-RU")] = "Идентификатор типа события некорректен",
			[(15, "en-US")] = "Minimal age should be less or equals to the maximum age",		[(15, "ru-RU")] = "Минимальный возраст должен быть меньше или равен максимальному возрасту",
			[(16, "en-US")] = "Event name is required",											[(16, "ru-RU")] = "Назвние события обязательно для заполнения",
			[(17, "en-US")] = "This person don't have this friend in his friend list",			[(17, "ru-RU")] = "У этого пользователя нет такого друга в его списке друзей",
			[(18, "en-US")] = "This person already have this friend in his friend list",		[(18, "ru-RU")] = "У этого пользователя уже есть этот друг в его списке людей",
			[(19, "en-US")] = "Chat is not exist in the database",								[(19, "ru-RU")] = "Чат не существует",
			[(20, "en-US")] = "Person is not a chat member",									[(20, "ru-RU")] = "Пользователь не относится к этому чату",
			[(21, "en-US")] = "Participant status is not correct",								[(21, "ru-RU")] = "Статус участника некорректен",
			[(22, "en-US")] = "Age is not valid",												[(22, "ru-RU")] = "Возраст некорректен",
			[(23, "en-US")] = "Coordinate is not valid",										[(23, "ru-RU")] = "Координаты некорректны",
			[(24, "en-US")] = "Person is already participate in this event",					[(24, "ru-RU")] = "Пользователь уже является участником данного события",
			[(25, "en-US")] = "No matching events found",										[(25, "ru-RU")] = "Не найдено подходящих событий",
			[(26, "en-US")] = "Person is not participate in this event",						[(26, "ru-RU")] = "Пользователь не участвует в указанном событии",
			[(27, "en-US")] = "No matching person found",										[(27, "ru-RU")] = "Не найдено подходящих пользователей",
			[(28, "en-US")] = "Page number should be bigger then 0",							[(28, "ru-RU")] = "Номер страницы должен быть больше 0",
			[(29, "en-US")] = "Page size should be bigger then 0",								[(29, "ru-RU")] = "Размер страницы должен быть больше 0",
			[(30, "en-US")] = "City id is not correct",											[(30, "ru-RU")] = "Неверный идентификатор города",
			[(31, "en-US")] = "City id should not be empty",									[(31, "ru-RU")] = "Идентификатор города не должен быть пустым",
			[(32, "en-US")] = "City id should be empty",										[(32, "ru-RU")] = "Идентификатор города должен быть пустым",
			[(33, "en-US")] = "Person with such login is already exist in the database",		[(33, "ru-RU")] = "Пользователем с таким логином уже существует",
			[(34, "en-US")] = "Token should not be empty",										[(34, "ru-RU")] = "Токен не должен быть пустым",
			[(35, "en-US")] = "Event can have maximum 3 types",									[(35, "ru-RU")] = "У события не может быть больше 3-х типов",
			[(36, "en-US")] = "At least 1 event type is required",								[(36, "ru-RU")] = "Необходимо указать хотя бы 1 тип события",
			[(37, "en-US")] = "Envent types should be distinct",								[(37, "ru-RU")] = "Типы события не должны повторяться",
			[(38, "en-US")] = "Token is required",												[(38, "ru-RU")] = "Необходимо указать токен",
			[(39, "en-US")] = "Feedback text should not be empty",								[(39, "ru-RU")] = "Необходимо ввести текст отзыва",
			[(40, "en-US")] = "Images count should be less than 10",							[(40, "ru-RU")] = "Количество картинок должно быть менее 10",
		};

		public static string GetErrorMessage(int code, string culture) => $"{{ \"errorCode\":{code}, \"message\":\"{Errors[(code, culture)]}\" }}";
	}
}
