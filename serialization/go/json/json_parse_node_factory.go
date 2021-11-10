package jsonserialization

import (
	"errors"

	absser "github.com/microsoft/kiota/abstractions/go/serialization"
)

// ParseNodeFactory implementation for JSON
type JsonParseNodeFactory struct {
}

// Creates a new JsonParseNodeFactory
func NewJsonParseNodeFactory() *JsonParseNodeFactory {
	return &JsonParseNodeFactory{}
}

func (f *JsonParseNodeFactory) GetValidContentType() (string, error) {
	return "application/json", nil
}
func (f *JsonParseNodeFactory) GetRootParseNode(contentType string, content []byte) (absser.ParseNode, error) {
	validType, err := f.GetValidContentType()
	if err != nil {
		return nil, err
	} else if contentType == "" {
		return nil, errors.New("contentType is empty")
	} else if contentType != validType {
		return nil, errors.New("contentType is not valid")
	} else {
		return NewJsonParseNode(content)
	}
}