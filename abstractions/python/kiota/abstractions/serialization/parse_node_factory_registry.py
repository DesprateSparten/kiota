from __future__ import annotations

from io import BytesIO
from typing import Dict

from .parse_node import ParseNode
from .parse_node_factory import ParseNodeFactory


class ParseNodeFactoryRegistry(ParseNodeFactory):
    """Holds a list of all the registered factories for the various types of nodes
    """
    CONTENT_TYPE_ASSOCIATED_FACTORIES: Dict[str, ParseNodeFactory] = {}

    __instance = None

    def __new__(cls, *args, **kwargs):
        """Default singleton instance of the registry to be used when registering new
        factories that should be available by default.

        Returns:
            [ParseNodeFactoryRegistry]: Default singleton instance of the class
        """
        if not ParseNodeFactoryRegistry.__instance:
            ParseNodeFactoryRegistry.__instance = object.__new__(cls)
        return ParseNodeFactoryRegistry.__instance

    def get_valid_content_type(self) -> str:
        raise Exception(
            "The registry supports multiple content types. Get the registered factory instead"
        )

    def get_root_parse_node(self, content_type: str, content: BytesIO) -> ParseNode:
        if not content_type:
            raise Exception("Content type cannot be null")
        if not content:
            raise Exception("Content cannot be null")

        factory = self.CONTENT_TYPE_ASSOCIATED_FACTORIES.get(content_type)
        if factory:
            return factory.get_root_parse_node(content_type, content)
        raise Exception(
            f"Content type {content_type} does not have a factory registered to be parsed"
        )
