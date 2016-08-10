(function( factory ) {
	if ( typeof define === "function" && define.amd ) {
		define( ["jquery", "../jquery.validate"], factory );
	} else if (typeof module === "object" && module.exports) {
		module.exports = factory( require( "jquery" ) );
	} else {
		factory( jQuery );
	}
}(function( $ ) {

/*
 * Translated default messages for the jQuery validation plugin.
 * Locale: IS (Icelandic; íslenska)
 */
$.extend( $.validator.messages, {
	required: "Потребно поле.",
	remote: "Поправи го ова поле.",
	maxlength: $.validator.format( "Максимум број на симболи: {0} " ),
	minlength: $.validator.format( "Минимален број на симболи: {0} " ),
	rangelength: $.validator.format( "Внесете број помеѓу {0} и {1}" ),
	email: "Ве молиме внесете валидна e-mail адреса.",
	url: "Внесете валиден URL.",
	date: "Внесете валиден датум.",
	number: "Внесете број.",
	digits: "Внесете исклучиво бројки.",
	equalTo: "Внесете го истотот повторно.",
	range: $.validator.format( "Внесете вредности помеѓу {0} и {1}." ),
	max: $.validator.format( "Внесете број помал од {0}." ),
	min: $.validator.format( "Внесете број поголем од {0}." ),
	creditcard: "Внесете го точниот број од кредитната картичка."
} );

}));